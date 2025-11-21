from openai import OpenAI
from azure.identity import DefaultAzureCredential, get_bearer_token_provider


token_provider = get_bearer_token_provider(DefaultAzureCredential(), "https://ai.azure.com/.default")

client = OpenAI(
    base_url="https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai",
    api_key=token_provider,
    default_query={"api-version": "2025-11-15-preview"}
)
response = client.responses.create(
    model="claude-sonnet-4-5",
    input="Write a one-sentence bedtime story about a unicorn."
)

print(f"Response from model: {response.model}:\n\n{response.output_text}")