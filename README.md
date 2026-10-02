# Expense Tracker

Мобільний застосунок для обліку особистих витрат, розроблений на C# з використанням .NET MAUI та архітектурного патерну MVVM.

## Опис проєкту

Застосунок дозволяє користувачу додавати та видаляти витрати, вказувати суму, валюту, категорію, опис і дату витрати.

Якщо витрата введена в USD або EUR, застосунок автоматично конвертує її у гривні за актуальним офіційним курсом НБУ. Усі витрати в застосунку зберігаються та підсумовуються в UAH.

Актуальні курси USD та EUR отримуються через API Національного банку України.

## Основні можливості

- Додавання нової витрати.
- Вибір валюти витрати: UAH, USD або EUR.
- Автоматична конвертація USD та EUR у гривні.
- Отримання актуальних курсів валют з API НБУ.
- Відображення поточного курсу USD та EUR.
- Відображення дати курсу.
- Перегляд історії витрат.
- Видалення витрати.
- Автоматичний підрахунок загальної суми витрат у гривнях.

## Використані технології

- C#
- .NET MAUI
- XAML
- MVVM
- REST API
- JSON
- API Національного банку України

## Структура проєкту

```text
Lab1
│
├── Models
│   ├── Expense.cs
│   └── CurrencyRate.cs
│
├── ViewModels
│   └── ExpensesViewModel.cs
│
├── Views
│   ├── ExpensesPage.xaml
│   └── ExpensesPage.xaml.cs
│
├── Services
│   ├── CurrencyConverter.cs
│   └── CurrencyRateService.cs
│
├── App.xaml
├── App.xaml.cs
├── AppShell.xaml
└── AppShell.xaml.cs
````

## Архітектура MVVM

У застосунку використовується архітектурний патерн MVVM (Model-View-ViewModel).

```mermaid
flowchart TD
    User[Користувач]

    View[View<br/>ExpensesPage.xaml]
    ViewModel[ViewModel<br/>ExpensesViewModel]
    Model[Model<br/>Expense]
    RateService[CurrencyRateService]
    Converter[CurrencyConverter]
    NBU[API НБУ]

    User --> View
    View <--> ViewModel

    ViewModel --> Model
    ViewModel --> RateService
    ViewModel --> Converter

    RateService --> NBU
```

### Model

Model відповідає за зберігання даних.

Основна модель `Expense` містить:

* Id
* Amount
* Category
* Description
* Date
* Currency

`CurrencyRate` використовується для представлення інформації про курс валюти.

### View

View відповідає за відображення інформації та взаємодію з користувачем.

Основна сторінка:

```text
ExpensesPage.xaml
```

На сторінці користувач може:

* переглядати загальну суму;
* переглядати курси валют;
* вводити нову витрату;
* вибирати валюту;
* переглядати історію витрат;
* видаляти витрати.

### ViewModel

`ExpensesViewModel` є посередником між View та іншими компонентами.

Він відповідає за:

* зберігання списку витрат;
* обробку команд;
* перевірку введених даних;
* отримання курсів валют;
* виклик конвертера;
* підрахунок загальної суми.

## UML діаграма класів

```mermaid
classDiagram

    class Expense {
        +int Id
        +decimal Amount
        +string Category
        +string Description
        +DateTime Date
        +string Currency
    }

    class CurrencyRate {
        +string Currency
        +decimal Rate
        +string Date
    }

    class ExpensesViewModel {
        -CurrencyConverter _currencyConverter
        -CurrencyRateService _currencyRateService
        -decimal _totalAmount
        -decimal _usdRate
        -decimal _eurRate
        -string _rateDate
        -string _amount
        -string _category
        -string _description
        -string _selectedCurrency
        -DateTime _currentDate

        +ObservableCollection~Expense~ Expenses
        +ObservableCollection~string~ Currencies
        +string Amount
        +string Category
        +string Description
        +DateTime CurrentDate
        +string SelectedCurrency
        +decimal TotalAmount
        +decimal UsdRate
        +decimal EurRate
        +string RateDate

        +ICommand AddExpenseCommand
        +ICommand DeleteExpenseCommand

        +LoadRatesAsync() Task
        -AddExpense() void
        -DeleteExpense(Expense) void
        -UpdateTotalAmount() void
        -ClearForm() void
    }

    class CurrencyConverter {
        +ConvertToUah(decimal amount, string currency, decimal usdRate, decimal eurRate) decimal
    }

    class CurrencyRateService {
        -HttpClient _httpClient
        +GetRatesAsync() Task~List~CurrencyRate~~
    }

    ExpensesViewModel --> Expense
    ExpensesViewModel --> CurrencyConverter
    ExpensesViewModel --> CurrencyRateService
    CurrencyRateService --> CurrencyRate
```

## UML діаграма компонентів

```mermaid
flowchart LR

    subgraph Application["Expense Tracker"]
        View["ExpensesPage.xaml<br/>View"]
        VM["ExpensesViewModel<br/>ViewModel"]

        subgraph Services["Services"]
            CRS["CurrencyRateService"]
            CC["CurrencyConverter"]
        end

        subgraph Models["Models"]
            E["Expense"]
            CR["CurrencyRate"]
        end
    end

    NBU["НБУ API"]

    View <--> VM

    VM --> E
    VM --> CRS
    VM --> CC

    CRS --> CR
    CRS --> NBU
```

## UML діаграма послідовності додавання витрати

Під час додавання витрати користувач вводить суму та вибирає валюту. Якщо валюта відрізняється від гривні, сума конвертується в UAH.

```mermaid
sequenceDiagram

    actor User as Користувач
    participant View as ExpensesPage
    participant VM as ExpensesViewModel
    participant Converter as CurrencyConverter
    participant Model as Expense

    User->>View: Вводить суму
    User->>View: Вибирає валюту
    User->>View: Натискає "Додати витрату"

    View->>VM: AddExpenseCommand
    VM->>Converter: ConvertToUah(amount, currency, rates)

    Converter-->>VM: Сума в UAH

    VM->>Model: Створює Expense
    VM->>VM: Додає Expense до Expenses
    VM->>VM: Оновлює TotalAmount

    VM-->>View: Оновлення даних
    View-->>User: Відображає витрату в UAH
```

## UML діаграма отримання курсів валют

```mermaid
sequenceDiagram

    participant View as ExpensesPage
    participant VM as ExpensesViewModel
    participant Service as CurrencyRateService
    participant NBU as API НБУ

    View->>VM: OnAppearing()
    VM->>Service: GetRatesAsync()
    Service->>NBU: HTTP GET запит
    NBU-->>Service: JSON з курсами валют

    Service->>Service: Фільтрує USD та EUR
    Service-->>VM: CurrencyRate[]

    VM->>VM: Зберігає UsdRate
    VM->>VM: Зберігає EurRate
    VM->>VM: Зберігає RateDate

    VM-->>View: Оновлення Binding
    View-->>View: Показує курси НБУ
```

## UML діаграма процесу конвертації

```mermaid
flowchart TD

    Start([Користувач вводить витрату])

    Input["Сума + Валюта"]

    Check{"Яка валюта?"}

    UAH["UAH<br/>Сума без змін"]

    USD["USD<br/>Сума × курс USD"]

    EUR["EUR<br/>Сума × курс EUR"]

    Result["Сума в UAH"]

    Expense["Створення Expense"]

    Total["Оновлення загальної суми"]

    Start --> Input
    Input --> Check

    Check -->|UAH| UAH
    Check -->|USD| USD
    Check -->|EUR| EUR

    UAH --> Result
    USD --> Result
    EUR --> Result

    Result --> Expense
    Expense --> Total
```

## Логіка роботи з валютами

Усі витрати після додавання зберігаються в гривнях.

Наприклад, якщо користувач вводить:

```text
Сума: 100
Валюта: USD
Курс: 41.50 UAH
```

застосунок виконує:

```text
100 × 41.50 = 4150 UAH
```

У моделі зберігається:

```text
Amount = 4150
Currency = UAH
```

Для EUR принцип такий самий:

```text
100 × курс EUR = сума в UAH
```

Якщо користувач вибирає UAH, конвертація не виконується:

```text
100 UAH = 100 UAH
```

## Взаємодія компонентів

Основний потік даних у застосунку:

```mermaid
flowchart TD

    A[Користувач] --> B[ExpensesPage.xaml]

    B --> C[ExpensesViewModel]

    C --> D{Додати витрату}

    D --> E[CurrencyConverter]

    E --> F[Конвертація в UAH]

    F --> G[Expense]

    G --> H[ObservableCollection Expenses]

    H --> I[TotalAmount]

    I --> B
```

Отримання курсу працює окремим потоком:

```mermaid
flowchart TD

    A[ExpensesPage] --> B[ExpensesViewModel]

    B --> C[CurrencyRateService]

    C --> D[HTTP GET]

    D --> E[API НБУ]

    E --> F[JSON]

    F --> C

    C --> B

    B --> G[UsdRate / EurRate]

    G --> A
```

## Приклад роботи

Користувач відкриває застосунок.

Застосунок отримує актуальні курси USD та EUR з API НБУ та показує їх у верхній частині сторінки.

Після цього користувач вводить:

```text
Сума: 50
Валюта: EUR
Категорія: Їжа
Опис: Кафе
Дата: 02.10.2026
```

Після натискання кнопки "Додати витрату" програма бере курс EUR, конвертує 50 EUR у гривні та додає отриману суму до списку витрат.

У списку відображається вже гривнева сума.

## API НБУ

Для отримання офіційних курсів валют використовується API Національного банку України:

```text
https://bank.gov.ua/NBUStatService/v1/statdirectory/exchangenew?json
```

API повертає дані у форматі JSON.

Застосунок використовує:

```text
cc            Код валюти
rate          Курс
exchangedate  Дата курсу
```
