# PiotrekPrezydent/StudentSchedule

## Przegląd

**Istotne pliki źródłowe:**

- [StudentScheduleBackend/Context.cs](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Context.cs)  
- [StudentScheduleBackend/Seeder.cs](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Seeder.cs)  
- [StudentScheduleClient/AdminPages/AdminEntityPage.cs](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/AdminPages/AdminEntityPage.cs)  
- [StudentScheduleClient/App.xaml.cs](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/App.xaml.cs)  
- [StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs)

---

## Cel i Zakres

Ten dokument zawiera kompleksowy przegląd repozytorium StudentSchedule — desktopowego systemu zarządzania planami zajęć studentów. System umożliwia instytucjom edukacyjnym zarządzanie zapisami studentów, programami nauczania, harmonogramami zajęć i przypisaniami sal w aplikacji WPF z kontrolą dostępu opartą na rolach.

System implementuje trójwarstwową architekturę składającą się z aplikacji klienckiej WPF, usług backendowych opartych na Entity Framework Core oraz bazy danych SQL Server.

Szczegóły dotyczące wybranych komponentów:

- Mechanizmy uwierzytelniania: [Authentication & Authorization](https://deepwiki.com/PiotrekPrezydent/StudentSchedule/3-authentication-and-authorization)
- Struktura modelu danych: [Data Model](https://deepwiki.com/PiotrekPrezydent/StudentSchedule/4-data-model)
- Funkcjonalność panelu administracyjnego: [Admin Interface](https://deepwiki.com/PiotrekPrezydent/StudentSchedule/5-admin-interface)

---

## Architektura Systemu

StudentSchedule wykorzystuje modularną architekturę z wyraźnym podziałem odpowiedzialności pomiędzy warstwami prezentacji, logiki biznesowej i dostępu do danych.

### Architektura komponentów wysokiego poziomu

#### Warstwa Klienta

- **App.xaml.cs** – punkt wejścia aplikacji  
- **LoginWindow** – interfejs logowania  
- **StudentWindow** – widok harmonogramu dla studentów  
- **AdminWindow** – panel administracyjny

#### Warstwa Logiki Biznesowej (Backend)

- **Repository** – operacje CRUD dla encji
- **Seeder.cs** – inicjalizacja bazy danych
- **CUItools (Command Line)** – narzędzia CLI (m.in. backup bazy danych)
- **Context.cs** – singleton `DbContext`

#### Warstwa Danych

- **SQL Server**
  - Migracje EF Core
  - Baza danych StudentSchedule

---

## Kluczowe Komponenty Systemu

### DbContext i Zarządzanie Połączeniem

Klasa `Context` implementuje bezpieczny wątkowo singleton do dostępu do bazy danych z dynamicznym generowaniem connection stringów na podstawie zalogowanego użytkownika:

- **Komponent:** `Context.Instance`
  - Odpowiedzialność: punkt dostępu Singleton
  - Kluczowe metody: `Initialize()`, `BuildConnectionString()`

- **Komponent:** Generator connection stringów
  - Odpowiedzialność: dostęp do bazy na podstawie roli
  - Wykorzystuje: `ReadWriteLogin` dla admina, `ReadLogin` dla studenta

- **Komponent:** Dostęp do encji
  - Odpowiedzialność: bezpieczne typowo pobieranie danych
  - Kluczowa metoda: `GetEntitiesByType()` – refleksja

System wykorzystuje różne poświadczenia SQL w zależności od roli:

- `ReadWriteLogin` / `ReadWritePass` – dostęp pełny (admin)
- `ReadLogin` / `ReadPass` – tylko do odczytu (student)

---

### Operacje CRUD (Ogólne)

Klasa `AdminEntityPage<T>` implementuje generyczny wzorzec zarządzania danymi administracyjnymi:

#### `AdminEntityPage<T>`

**Pola:**

- `Repository<T> _repository`
- `List<KeyValuePair> _filters`

**Metody:**

- `OnEdit()`
- `OnDelete()`
- `OnAdd()`
- `OnFilter()`

#### `Repository<T>`

| Metoda         | Opis                          |
|----------------|-------------------------------|
| `GetAll()`     | Pobiera wszystkie encje       |
| `Add(entity)`  | Dodaje nową encję             |
| `Update(entity)`| Aktualizuje istniejącą encję |
| `Delete(id)`   | Usuwa encję na podstawie ID   |

---

### Komponent ShowColumnsPopup

- `ReadedValues` – lista odczytanych wartości
- `GenerateColumns(entityType, popupType)` – dynamiczne tworzenie kolumn na podstawie typu
- `SetReadedValues()` – przypisanie wartości do kontrolek

#### Abstrakcyjna Klasa Entity

- `int Id` – pole identyfikatora
- `CreateFromKVP<T>(kvps)` – fabryka tworząca encję na podstawie KeyValuePair

---

## Dynamiczne Generowanie UI

Klasa `ShowColumnsPopup` dynamicznie generuje formularze w oparciu o właściwości encji wykorzystując refleksję:

| Typ właściwości       | Typ kontrolki UI            | Generowana kontrolka                 |
|------------------------|-----------------------------|--------------------------------------|
| Właściwości klucza obcego | `[ForeignKeyOf(Type)]`     | ComboBox z ID encji                  |
| Boolean                | `bool`                      | CheckBox                             |
| Czas                   | `TimeSpan`                  | TextBox z walidacją formatu          |
| Liczbowe               | `int`, `double`, `decimal`  | TextBox z walidacją liczbową         |
| Tekstowe               | `string`                    | TextBox                              |

---

## Inicjalizacja i Ziarno Danych

### Proces uruchamiania aplikacji

W trybie debug aplikacja automatycznie usuwa i tworzy bazę danych oraz zasiewa dane:

Źródła:  
- [App.xaml.cs (23–40)](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/App.xaml.cs#L23-L40)  
- [Seeder.cs (9–48)](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Seeder.cs#L9-L48)

### Zarządzanie użytkownikami bazy

Proces ziarna tworzy konta logowania SQL i przypisuje odpowiednie role:

- `ReadWriteLogin` – rola `db_owner` (pełny dostęp)
- `ReadLogin` – rola `db_datareader` (tylko odczyt)

---

## Kluczowe Wzorce Architektoniczne

### Wzorzec Repositorium

System implementuje generyczny wzorzec repository – `Repository<T>` udostępnia jednolite operacje CRUD dla każdej encji dziedziczącej po klasie bazowej `Entity`.

### Singleton DbContext

Klasa `Context` używa bezpiecznego singletona z lazy inicjalizacją – gwarantuje jeden wspólny punkt dostępu do bazy danych.

### Dynamiczne Formularze

Warstwa UI używa refleksji do automatycznego generowania kontrolek wejściowych na podstawie typu danych i atrybutów, eliminując potrzebę ręcznego tworzenia formularzy dla każdej encji.

### Dostęp do danych oparty na rolach

Autoryzacja użytkownika określa poziom dostępu do bazy danych:
- admin: pełny dostęp
- student: tylko odczyt

---

Źródła:  
- [Context.cs (25–92)](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleBackend/Context.cs#L25-L92)  
- [AdminEntityPage.cs (10–20)](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/AdminPages/AdminEntityPage.cs#L10-L20)  
- [ShowColumnsPopup.xaml.cs (68–165)](https://github.com/PiotrekPrezydent/StudentSchedule/blob/3c991c28/StudentScheduleClient/Popups/ShowColumnsPopup.xaml.cs#L68-L165)
