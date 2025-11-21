# TypeScript - Claude Sonnet 4.5 Example

## Prerequisites

- Node.js 22+
- Azure CLI (logged in)

## Setup

```bash
# Install dependencies
npm install
```

Create a `.env` file in this directory with the following content:

```
FOUNDRY_ENDPOINT="https://YOUR-RESOURCE-NAME.services.ai.azure.com/api/projects/YOUR-PROJECT-NAME/openai"
```

## Run

```bash
# Run with tsx
npm start

# Or directly
npx tsx claude-openai-responses.ts
```

## Files

- `claude-openai-responses.ts` - Main TypeScript code with EntraID auth
- `package.json` - Node.js dependencies
- `tsconfig.json` - TypeScript configuration
