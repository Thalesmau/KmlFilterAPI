# **KML Filter API**

Uma API RESTful desenvolvida em **.NET** para manipulação e exportação de arquivos KML, permitindo filtragem e geração de novos arquivos baseados em critérios personalizados.

## **Funcionalidades**

- **Filtrar e Exportar KML**:
  - Gera um novo arquivo KML com base nos filtros aplicados.
  - Endpoint: `/api/placemarks/export`.

- **Listar Elementos Filtrados (JSON)**:
  - Retorna elementos filtrados no formato JSON.
  - Endpoint: `/api/placemarks`.

- **Obter Elementos Disponíveis para Filtragem**:
  - Retorna valores únicos para os campos `CLIENTE`, `SITUAÇÃO` e `BAIRRO`.
  - Endpoint: `/api/placemarks/filters`.

## **Configuração do Projeto**

1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/KmlFilterAPI.git
   cd KmlFilterAPI
   ```
2. Instale as dependências necessárias
  ```bash
  dotnet restore
  ```

3. Compile o projeto
  ```bash
  dotnet build
  ```

4. Inicie o servidor
  ```bash
  dotnet run
  ``` 
