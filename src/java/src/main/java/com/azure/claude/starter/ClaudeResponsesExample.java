package com.azure.claude.starter;

import com.azure.identity.AuthenticationUtil;
import com.azure.identity.DefaultAzureCredentialBuilder;
import com.openai.client.OpenAIClient;
import com.openai.client.okhttp.OpenAIOkHttpClient;
import com.openai.credential.BearerTokenCredential;
import com.openai.models.ResponsesModel;
import com.openai.models.responses.Response;
import com.openai.models.responses.ResponseCreateParams;

import java.util.function.Supplier;

/**
 * Claude Sonnet 4.5 - Responses API Example with Azure Entra ID Authentication
 *
 * This example demonstrates how to use the OpenAI Java SDK to interact with Claude models
 * hosted on Microsoft Azure AI Foundry using Azure Entra ID authentication.
 *
 * <p>Configuration requirements:
 * <ul>
 *   <li>Azure resource name and project name in the base URL</li>
 *   <li>API version query parameter (2025-11-15-preview)</li>
 *   <li>Azure credentials (via DefaultAzureCredential)</li>
 *   <li>Model deployment name (claude-sonnet-4-5)</li>
 * </ul>
 *
 * <p>The implementation uses:
 * <ul>
 *   <li>OpenAIOkHttpClient for HTTP communication</li>
 *   <li>BearerTokenCredential for Azure Entra ID token-based authentication</li>
 *   <li>ResponseCreateParams to configure the API request</li>
 * </ul>
 */
public class ClaudeResponsesExample {

    public static void main(String[] args) {
        System.out.println("Claude Sonnet 4.5 - Responses API with EntraID");

        // Get Azure token using DefaultAzureCredential
        var azureCredential = new DefaultAzureCredentialBuilder().build();

        // Foundry endpoint configuration
        String baseUrl = "https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai";
        String apiVersion = "2025-11-15-preview";
        String model = "claude-sonnet-4-5";

        Supplier<String> bearerTokenSupplier = AuthenticationUtil.getBearerTokenSupplier(azureCredential, "https://ai.azure.com/.default");

        OpenAIClient client = OpenAIOkHttpClient.builder()
                .baseUrl(baseUrl)
                // Set the Azure Entra ID
                .credential(BearerTokenCredential.create(bearerTokenSupplier))
                .putQueryParam("api-version", apiVersion)
                .build();

        ResponseCreateParams responseCreateParams = ResponseCreateParams.builder()
                .model(ResponsesModel.ofString(model))
                .input(ResponseCreateParams.Input.ofText("Explain quantum computing in simple terms"))
                .maxOutputTokens(1000)
                .build();

        Response response = client.responses().create(responseCreateParams);

        System.out.println("Response from model: " + model);
        System.out.println(response.output());
    }
}
