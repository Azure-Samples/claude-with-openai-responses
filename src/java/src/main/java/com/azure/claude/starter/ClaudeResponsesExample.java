package com.azure.claude.starter;

import com.azure.core.credential.AccessToken;
import com.azure.core.credential.TokenRequestContext;
import com.azure.identity.DefaultAzureCredentialBuilder;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.MediaType;
import okhttp3.Response;

import java.io.IOException;

/**
 * Claude Sonnet 4.5 - Responses API with EntraID Authentication
 * This demonstrates using Azure Identity (EntraID) for keyless authentication.
 * 
 * Note: OpenAI Java SDK 4.8.0 doesn't support adding query parameters required 
 * for Azure AI Foundry endpoints. This implementation uses OkHttp directly.
 * 
 * For production Azure AI Foundry applications, consider using:
 * - Azure AI Projects SDK for Java (when available)
 * - Direct HTTP calls with OkHttp (as shown here)
 */
public class ClaudeResponsesExample {

    public static void main(String[] args) throws IOException {
        System.out.println("Claude Sonnet 4.5 - Responses API with EntraID\n");

        String endpoint = "https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai/responses?api-version=2025-11-15-preview";
        
        // Use DefaultAzureCredential for EntraID authentication
        var credential = new DefaultAzureCredentialBuilder().build();
        TokenRequestContext tokenContext = new TokenRequestContext().addScopes("https://ai.azure.com/.default");
        AccessToken token = credential.getTokenSync(tokenContext);

        // Create JSON request body
        String jsonBody = """
            {
                "model": "claude-sonnet-4-5",
                "input": "Write a one-sentence bedtime story about a unicorn.",
                "max_output_tokens": 1000
            }
            """;

        // Make HTTP request with OkHttp
        OkHttpClient client = new OkHttpClient();
        Request request = new Request.Builder()
                .url(endpoint)
                .addHeader("Authorization", "Bearer " + token.getToken())
                .addHeader("Content-Type", "application/json")
                .post(RequestBody.create(jsonBody, MediaType.get("application/json")))
                .build();

        try (Response response = client.newCall(request).execute()) {
            if (response.isSuccessful() && response.body() != null) {
                String responseBody = response.body().string();
                
                // Parse JSON response using Jackson
                ObjectMapper objectMapper = new ObjectMapper();
                JsonNode root = objectMapper.readTree(responseBody);
                
                System.out.println("Response from model: " + root.get("model").asText() + ":\n");
                
                // Navigate the response structure:
                // root.output[0] (message object) -> .content[0] (content object) -> .text
                JsonNode output = root.path("output");
                if (output.isArray() && output.size() > 0) {
                    JsonNode message = output.get(0);
                    JsonNode content = message.path("content");
                    if (content.isArray() && content.size() > 0) {
                        JsonNode contentItem = content.get(0);
                        JsonNode text = contentItem.path("text");
                        if (!text.isMissingNode()) {
                            System.out.println(text.asText());
                        }
                    }
                }
            } else {
                System.err.println("Error: " + response.code() + " - " + 
                    (response.body() != null ? response.body().string() : "No response body"));
            }
        }
    }
}
