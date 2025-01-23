Storage Service API
Opis
Storage Service API to prosty serwis API umożliwiający przechowywanie danych w pamięci w formacie JSON, z obsługą różnych formatów wejściowych: JSON, XML oraz YAML. API udostępnia możliwość konwersji danych wejściowych do formatu JSON i ich późniejszego pobierania.

Serwis jest zintegrowany z Swagger UI, co umożliwia łatwe testowanie dostępnych endpointów.

Funkcjonalności
Akceptowanie danych wejściowych w formatach:
JSON (application/json)
XML (application/xml, text/xml)
YAML (application/yaml, text/yaml)
Automatyczna konwersja danych wejściowych do formatu JSON.
Przechowywanie danych w pamięci.
Możliwość pobierania zapisanych danych według indeksu.
Wbudowana dokumentacja OpenAPI dostępna w Swagger UI.
Wymagania
.NET 6 lub nowszy
Zależności:
Microsoft.AspNetCore
Microsoft.OpenApi
YamlDotNet
Newtonsoft.Json
Instalacja
Sklonuj repozytorium:

bash
Kopiuj
Edytuj
git clone <repository-url>
cd <repository-directory>
Przygotuj środowisko:

Upewnij się, że masz zainstalowany .NET 6 lub nowszy.
Uruchom aplikację:

bash
Kopiuj
Edytuj
dotnet run
Swagger UI będzie dostępny pod adresem:

bash
Kopiuj
Edytuj
http://localhost:<port>/swagger
Endpointy
1. GET /
Opis: Przekierowanie na Swagger UI.
Odpowiedź:
Przekierowuje na /swagger.

2. POST /data
Opis: Przesyłanie danych w formacie JSON/XML/YAML.
Parametry:

Treść żądania (Body) w jednym z obsługiwanych formatów:
JSON: application/json
XML: application/xml, text/xml
YAML: application/yaml, text/yaml
Przykładowe dane wejściowe:

JSON:
json
Kopiuj
Edytuj
{
  "key": "value"
}
XML:
xml
Kopiuj
Edytuj
<root>
  <key>value</key>
</root>
YAML:
yaml
Kopiuj
Edytuj
key: value
Odpowiedzi:

201 Created:
Dane zostały zapisane. Zwraca indeks zapisanych danych.
json
Kopiuj
Edytuj
{
  "Index": 1
}
400 Bad Request:
Nieobsługiwany format danych.
Nieprawidłowy format wejściowy.
3. GET /data/{index}
Opis: Pobieranie zapisanych danych według indeksu.
Parametry:

{index}: Numer indeksu danych.
Odpowiedzi:

200 OK:
Zwraca dane w formacie JSON.
404 Not Found:
Dane o podanym indeksie nie istnieją.
Swagger UI
Aby zobaczyć szczegółową dokumentację API i przetestować endpointy:

Uruchom aplikację.
Przejdź do:
bash
Kopiuj
Edytuj
http://localhost:<port>/swagger
Przykładowe użycie
cURL
Przesyłanie danych (JSON):

bash
Kopiuj
Edytuj
curl -X POST http://localhost:<port>/data \
-H "Content-Type: application/json" \
-d '{"key":"value"}'
Pobieranie danych:

bash
Kopiuj
Edytuj
curl http://localhost:<port>/data/1
Swagger
Otwórz Swagger UI w przeglądarce.
Wybierz odpowiedni endpoint.
Przetestuj API bezpośrednio z interfejsu.
Autorzy
Kod przygotowany z wykorzystaniem frameworka ASP.NET Core. Zintegrowano funkcje konwersji i walidacji przy użyciu bibliotek Newtonsoft.Json oraz YamlDotNet.
