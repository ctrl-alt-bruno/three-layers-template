# API Error Handling Documentation

Este projeto implementa um sistema robusto de gerenciamento de erros de API seguindo as melhores práticas REST.

## Códigos de Status HTTP Implementados

### 200 OK
- **Quando usar**: Requisições GET bem-sucedidas
- **Exemplo**: `GET /api/products` retorna lista de produtos

### 201 Created  
- **Quando usar**: Recursos criados com sucesso
- **Exemplo**: `POST /api/products` cria um novo produto
- **Resposta**: Inclui o objeto criado e header `Location`

### 204 No Content
- **Quando usar**: Operações de atualização/exclusão bem-sucedidas
- **Exemplo**: `PUT /api/products/{id}` ou `DELETE /api/products/{id}`

### 400 Bad Request
- **Quando usar**: Erros de validação de modelo/entrada
- **Implementação**: `CreateCustomActionResult(ModelState)`
- **Formato de resposta**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "See 'errors' for details.",
  "errors": {
    "messages": ["Campo obrigatório não informado"]
  }
}
```

### 401 Unauthorized
- **Quando usar**: Autenticação requerida mas não fornecida/inválida
- **Exception**: `UnauthorizedException`
- **Implementação**: `CreateUnauthorizedResult(message)`

### 403 Forbidden
- **Quando usar**: Usuário autenticado mas sem permissões
- **Exception**: `ForbiddenException`
- **Implementação**: `CreateForbiddenResult(message)`

### 404 Not Found
- **Quando usar**: Recurso não encontrado
- **Exception**: `EntityNotFoundException`
- **Implementação**: `CreateNotFoundResult(message)`
- **Formato de resposta**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found", 
  "status": 404,
  "detail": "Product with id '123' was not found.",
  "entityName": "Product",
  "entityId": "123"
}
```

### 409 Conflict
- **Quando usar**: Violação de regras de negócio
- **Exception**: `BusinessRuleException`
- **Implementação**: `CreateConflictResult(message)`
- **Exemplos**:
  - Tentativa de criar fornecedor com documento duplicado
  - Tentativa de excluir fornecedor com produtos associados

### 422 Unprocessable Entity
- **Quando usar**: Erros de validação semântica
- **Exception**: `ValidationException`
- **Implementação**: `CreateUnprocessableEntityResult(errors)`

### 500 Internal Server Error
- **Quando usar**: Erros não tratados/inesperados
- **Implementação**: Middleware de exceção global

## Arquitetura de Tratamento de Erros

### 1. Exceções de Domínio (Business Layer)

```csharp
// Localização: src/ThreeLayers.Business/Exceptions/

BusinessRuleException     // 409 Conflict
EntityNotFoundException   // 404 Not Found  
ValidationException      // 422 Unprocessable Entity
UnauthorizedException   // 401 Unauthorized
ForbiddenException     // 403 Forbidden
```

### 2. CustomControllerBase (WebAPI Layer)

Fornece métodos para criar respostas padronizadas:

```csharp
protected ActionResult CreateNotFoundResult(string? message = null)
protected ActionResult CreateConflictResult(string? message = null)
protected ActionResult CreateUnprocessableEntityResult(IEnumerable<string>? errors = null)
protected ActionResult CreateUnauthorizedResult(string? message = null)
protected ActionResult CreateForbiddenResult(string? message = null)
```

### 3. Exception Middleware

O middleware `ExceptionHandlerExtensions` captura exceções não tratadas e mapeia para códigos HTTP apropriados:

```csharp
EntityNotFoundException     → 404 Not Found
BusinessRuleException      → 409 Conflict  
ValidationException        → 422 Unprocessable Entity
UnauthorizedException     → 401 Unauthorized
ForbiddenException        → 403 Forbidden
ArgumentException         → 400 Bad Request
Outras exceções           → 500 Internal Server Error
```

### 4. Sistema de Notificações

Para erros de validação que não devem interromper o fluxo:

```csharp
protected void Notify(string message)
```

## Formato de Resposta Padrão (RFC 7807)

Todas as respostas de erro seguem o padrão Problem Details:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.x",
  "title": "Título do erro",
  "status": 400,
  "detail": "Descrição detalhada do erro",
  "instance": "/api/products/123",
  "traceId": "0HN7K5V5H9234",
  // Propriedades específicas por tipo de erro
}
```

## Logging Estruturado

O sistema implementa logging diferenciado por gravidade:

- **Errors (500+)**: Exceções não tratadas com stack trace completo
- **Warnings (400-499)**: Erros de cliente com informações contextuais
- **Information**: Operações normais

Propriedades de contexto incluídas no log:
- TraceId
- ClientIp  
- UserAgent
- Headers
- CorrelationId

## Exemplos de Uso

### Nos Controllers
```csharp
// 404 Not Found
if (product == null)
    return CreateNotFoundResult($"Product with id '{id}' was not found.");

// 409 Conflict  
if (id != productUpdate.Id)
    return CreateConflictResult("The ID in the URL does not match the ID in the request body.");
```

### Nos Services
```csharp
// Lançar exceção que será convertida automaticamente
if (productToUpdate == null)
    throw new EntityNotFoundException("Product", product.Id);

if (supplierExists)
    throw new BusinessRuleException("A supplier with this document already exists.");
```

## Testes

Para testar o sistema de erros, execute:

```bash
./test_api_errors.sh
```

O script testa cenários como:
- GET recurso inexistente (404)
- POST dados inválidos (400)  
- PUT com IDs não coincidentes (409)
- GET lista vazia (200)

## Benefícios da Implementação

1. **Consistência**: Todas as respostas seguem o mesmo padrão
2. **Rastreabilidade**: Logs estruturados facilitam debugging
3. **Conformidade**: Segue padrões REST e RFC 7807
4. **Manutenibilidade**: Código limpo e bem organizado
5. **Extensibilidade**: Fácil adicionar novos tipos de erro
6. **Monitoramento**: Métricas por tipo de erro
7. **Experiência do Cliente**: Mensagens de erro claras e acionáveis