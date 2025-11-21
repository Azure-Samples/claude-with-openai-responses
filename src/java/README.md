# Java - Claude Sonnet 4.5 Example

## Prerequisites

- Java 17+
- Maven 3.6+
- Azure CLI (logged in)

## Setup

```bash
# Install dependencies
mvn clean install
```

## Run

```bash
# Run with Maven
mvn exec:java

# Or compile and run
mvn compile
mvn exec:java -Dexec.mainClass="com.azure.claude.starter.ClaudeResponsesExample"
```

## Files

- `ClaudeResponsesExample.java` - Main Java code with EntraID authentication
- `pom.xml` - Maven project configuration and dependencies
