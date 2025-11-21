# Python - Claude Sonnet 4.5 Example

## Prerequisites

- Python 3.11+
- Azure CLI (logged in)

## Setup

```bash
# Create virtual environment
python -m venv .venv

# Activate virtual environment (Windows)
.\.venv\Scripts\Activate.ps1

# Install dependencies
pip install -r requirements.txt
```

## Run

```bash
# Run the Responses API example
python claude-openai-responses2.py
```

## Files

- `claude-openai-responses.py` - Uses Azure AI Projects SDK
- `claude-openai-responses2.py` - Uses OpenAI SDK directly (recommended)
- `requirements.txt` - Python dependencies
