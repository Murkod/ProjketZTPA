
# System Mikrousług do Przechowywania i Konwersji Danych

Repozytorium zawiera dwa niezależne serwisy .NET 8 Minimal API:
1. **Storage Service** - przechowuje dane w formacie JSON.
2. **Conversion Gateway** - konwertuje dane między formatami JSON, XML i YAML.

---


### Składniki
| Serwis              | Port  | Opis                                |
|---------------------|-------|-------------------------------------|
| **Storage Service** | 5000  | Przechowuje dane w formacie JSON    |
| **Conversion Gateway** | 5001 | Konwertuje dane do XML/YAML         |

---

## Funkcje

### Storage Service
- Przyjmuje dane w formatach: JSON, XML, YAML.
- Przechowuje dane w formacie JSON.
- Udostępnia dane przez indeks.

### Conversion Gateway
- Konwertuje dane z JSON do XML/YAML.
- Integruje się z Storage Service.
- Obsługuje formaty wyjściowe: JSON, XML, YAML.

---

## Uruchomienie

### Wymagania
- .NET 8 SDK
- Porty 5000 i 5001 wolne

### Kroki
1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/twoje-repo/mikrousługi.git
   cd mikrousługi
   ```

2. Uruchom Storage Service:
   ```bash
   cd StorageService
   dotnet run --urls "http://localhost:5000"
   ```

3. Uruchom Conversion Gateway (w osobnym terminalu):
   ```bash
   cd ConversionGateway
   dotnet run --urls "http://localhost:5001"
   ```

---

## Testowanie

### Przykładowe żądania

#### 1. Zapisz dane w Storage Service
```bash
curl -X POST -H "Content-Type: application/json" -d '{"temperature":25}' http://localhost:5000/data
```

#### 2. Pobierz dane w formacie XML przez Conversion Gateway
```bash
curl http://localhost:5001/convert/1/xml
```

#### 3. Pobierz dane w formacie YAML
```bash
curl http://localhost:5001/convert/1/yaml
```

---

## Dokumentacja API

### Storage Service
- Swagger UI: `http://localhost:5000/swagger`
- Endpointy:
  - `POST /data` - Zapisz dane
  - `GET /data/{index}` - Pobierz dane

### Conversion Gateway
- Swagger UI: `http://localhost:5001/swagger`
- Endpointy:
  - `GET /convert/{index}/{format}` - Konwertuj dane



---

**Autorzy:**  
- Marek Matkowski  
- Patryk Banaszak  




### Co zawiera ten README:
- Opis architektury systemu.
- Instrukcje uruchomienia obu serwisów.
- Przykłady użycia API.
- Linki do dokumentacji Swagger.
- Informacje o licencji i autorach.
