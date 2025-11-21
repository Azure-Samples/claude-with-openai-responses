package com.azure.claude.starter;

import com.azure.core.credential.TokenRequestContext;
import com.azure.identity.DefaultAzureCredentialBuilder;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import okhttp3.*;

import java.io.IOException;

/**
 * Claude Sonnet 4.5 - Responses API with Direct HTTP and Azure EntraID
 * Uses direct HTTP requests with OkHttp for maximum compatibility with Microsoft Foundry.
 * 
 * Note: The OpenAI Java SDK's Responses API currently has compatibility issues with
 * Microsoft Foundry endpoints. This implementation uses direct HTTP as a workaround.
 */
public class ClaudeResponsesExample {

    public static void main(String[] args) throws IOException {
        System.out.println("Claude Sonnet 4.5 - Responses API with EntraID\n");

        // Get Azure token using DefaultAzureCredential
        var azureCredential = new DefaultAzureCredentialBuilder().build();
        var tokenContext = new TokenRequestContext().addScopes("https://ai.azure.com/.default");
        var azureToken = azureCredential.getTokenSync(tokenContext);

        // Foundry endpoint configuration
        String baseUrl = "https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai";
        String apiVersion = "2025-11-15-preview";
        String model = "claude-sonnet-4-5";

        // Build request body
        String requestBody = String.format("""
                {
                    "model": "%s",
                    "input": "Write a one-sentence bedtime story about a unicorn.",
                    "max_output_tokens": 1000
                }
                """, model);

        // Create HTTP request
        OkHttpClient client = new OkHttpClient();
        Request request = new Request.Builder()
                .url(baseUrl + "/responses?api-version=" + apiVersion)
                .addHeader("Authorization", "Bearer " + azureToken.getToken())
                .addHeader("Content-Type", "application/json")
                .post(RequestBody.create(requestBody, MediaType.get("application/json")))
                .build();

        // Execute request and parse response
        try (okhttp3.Response response = client.newCall(request).execute()) {
            if (!response.isSuccessful()) {
                throw new IOException("Request failed: " + response.code() + " " + response.message());
            }

            ObjectMapper mapper = new ObjectMapper();
            JsonNode jsonResponse = mapper.readTree(response.body().string());

            System.out.println("Response from model: " + jsonResponse.get("model").asText() + ":\n");

            // Extract and display output text
            JsonNode output = jsonResponse.get("output");
            if (output != null && output.isArray()) {
                for (JsonNode item : output) {
                    JsonNode content = item.get("content");
                    if (content != null && content.isArray()) {
                        for (JsonNode contentItem : content) {
                            if (contentItem.has("text")) {
                                System.out.println(contentItem.get("text").asText());
                            }
                        }
                    }
                }
            }
        }
    }
}
