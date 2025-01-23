# Storage Service API

## Opis
**Storage Service API** to prosty serwis API umożliwiający przechowywanie danych w pamięci w formacie JSON, z obsługą różnych formatów wejściowych: JSON, XML oraz YAML. API udostępnia możliwość konwersji danych wejściowych do formatu JSON i ich późniejszego pobierania.

Serwis jest zintegrowany z **Swagger UI**, co umożliwia łatwe testowanie dostępnych endpointów.

---

## Funkcjonalności
- Akceptowanie danych wejściowych w formatach:
  - JSON (`application/json`)
  - XML (`application/xml`, `text/xml`)
  - YAML (`application/yaml`, `text/yaml`)
- Automatyczna konwersja danych wejściowych do formatu JSON.
- Przechowywanie danych w pamięci.
- Możliwość pobierania zapisanych danych według indeksu.
- Wbudowana dokumentacja OpenAPI dostępna w Swagger UI.

---

## Wymagania
- .NET 6 lub nowszy
- Zależności:
  - `Microsoft.AspNetCore`
  - `Microsoft.OpenApi`
  - `YamlDotNet`
  - `Newtonsoft.Json`

---

## Instalacja
1. Sklonuj repozytorium:
   ```bash
   git clone <repository-url>
   cd <repository-directory>
   ```

2. Przygotuj środowisko:
   - Upewnij się, że masz zainstalowany .NET 6 lub nowszy.

3. Uruchom aplikację:
   ```bash
   dotnet run
   ```

4. Swagger UI będzie dostępny pod adresem:
   ```
   http://localhost:<port>/swagger
   ```

---

## Endpointy

### 1. **POST /data**  
**Opis:** Przesyłanie danych w formacie JSON/XML/YAML.  
**Parametry:**  
- Treść żądania (`Body`) w jednym z obsługiwanych formatów:
  - **JSON**: `application/json`
  - **XML**: `application/xml`, `text/xml`
  - **YAML**: `application/yaml`, `text/yaml`

**Przykładowe dane wejściowe:**
- **JSON:**
  ```json
  {
    "key": "value"
  }
  ```
- **XML:**
  ```xml
  <root>
    <key>value</key>
  </root>
  ```
- **YAML:**
  ```yaml
  key: value
  ```

**Odpowiedzi:**
- **201 Created:**  
  Dane zostały zapisane. Zwraca indeks zapisanych danych.  
  ```json
  {
    "Index": 1
  }
  ```
- **400 Bad Request:**  
  - Nieobsługiwany format danych.
  - Nieprawidłowy format wejściowy.

---

### 2. **GET /data/{index}**  
**Opis:** Pobieranie zapisanych danych według indeksu.  
**Parametry:**  
- `{index}`: Numer indeksu danych.

**Odpowiedzi:**
- **200 OK:**  
  Zwraca dane w formacie JSON.
- **404 Not Found:**  
  Dane o podanym indeksie nie istnieją.

---

## Swagger UI
Aby zobaczyć szczegółową dokumentację API i przetestować endpointy:
1. Uruchom aplikację.
2. Przejdź do:
   ```
   http://localhost:<port>/swagger
   ```

---

## Przykładowe użycie

### cURL
1. **Przesyłanie danych (JSON):**
   ```bash
   curl -X POST http://localhost:<port>/data \
   -H "Content-Type: application/json" \
   -d '{"key":"value"}'
   ```

2. **Pobieranie danych:**
   ```bash
   curl http://localhost:<port>/data/1
   ```

### Swagger
1. Otwórz Swagger UI w przeglądarce.
2. Wybierz odpowiedni endpoint.
3. Przetestuj API bezpośrednio z interfejsu.

---

## Autorzy:
## Marek Matkowski
## Patry Banaszak

##Kod przygotowany z wykorzystaniem frameworka .NET Zintegrowano funkcje konwersji i walidacji przy użyciu bibliotek **Newtonsoft.Json** oraz **YamlDotNet**.



