package main

import (
	"context"
	"fmt"
	"log"
	"net/http"

	"github.com/Azure/azure-sdk-for-go/sdk/azcore/policy"
	"github.com/Azure/azure-sdk-for-go/sdk/azcore/runtime"
	"github.com/Azure/azure-sdk-for-go/sdk/azidentity"
	"github.com/openai/openai-go/v3"
	"github.com/openai/openai-go/v3/option"
	"github.com/openai/openai-go/v3/responses"
)

func main() {
	fmt.Println("Claude Sonnet 4.5 - Responses API with EntraID")

	baseURL := "https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai"

	client := newClientUsingEntraAuthentication(baseURL)

	// Create response using Claude model
	resp, err := client.Responses.New(context.TODO(), responses.ResponseNewParams{
		Model: "claude-sonnet-4-5",
		Input: responses.ResponseNewParamsInputUnion{
			OfString: openai.String("Write a one-sentence bedtime story about a unicorn."),
		},
	})

	if err != nil {
		log.Fatalf("Failed to create response: %s", err)
	}

	fmt.Printf("Response from model: %s:\n\n%s\n", resp.Model, resp.OutputText())
}

type policyAdapter option.MiddlewareNext

func (mp policyAdapter) Do(req *policy.Request) (*http.Response, error) {
	return option.MiddlewareNext(mp)(req.Raw())
}

func newClientUsingEntraAuthentication(baseURL string) openai.Client {
	const scope = "https://ai.azure.com/.default"

	// Use DefaultAzureCredential for EntraID authentication
	tokenCredential, err := azidentity.NewDefaultAzureCredential(nil)
	if err != nil {
		log.Fatalf("Failed to create DefaultAzureCredential: %s", err)
	}

	bearerTokenPolicy := runtime.NewBearerTokenPolicy(tokenCredential, []string{scope}, nil)

	// Initialize OpenAI client with Azure AI Foundry endpoint
	client := openai.NewClient(
		option.WithBaseURL(baseURL),
		option.WithQuery("api-version", "2025-11-15-preview"),
		option.WithMiddleware(func(req *http.Request, next option.MiddlewareNext) (*http.Response, error) {
			pipeline := runtime.NewPipeline("claude-starter", "", runtime.PipelineOptions{}, &policy.ClientOptions{
				InsecureAllowCredentialWithHTTP: true,
				PerRetryPolicies: []policy.Policy{
					bearerTokenPolicy,
					policyAdapter(next),
				},
			})

			req2, err := runtime.NewRequestFromRequest(req)
			if err != nil {
				return nil, err
			}

			return pipeline.Do(req2)
		}),
	)

	return client
}
