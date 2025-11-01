# Соглашения по проекту PerceptionWorld

**Цель**: унифицировать стиль кода, структуру проекта и подходы к разработке, чтобы:
- снизить когнитивную нагрузку при чтении кода;
- избежать дублирования решений;
- ускорить адаптивность новых участников (включая YandexGPT 5.1 Pro (Алиса), как ассистента).

## 1. Структура проекта

```
PerceptionWorld/
├── Assets/                 # Сырые ресурсы (текстуры, модели, звуки)
├── Content/                # Обработанные ресурсы для MonoGame
├── Utils/                  # Вспомогательные классы
│   ├── Res.cs              # Загрузка и кэширование ресурсов
│   └── State.cs             # Глобальные ссылки (GraphicsDevice, SpriteBatch и т. п.)
├── World/                  # Логика мира (чанки, генерация)
│   ├── ChunkData.cs
│   └── World.cs
├── Camera.cs               # Камера и матрицы вида/проекции
├── BoxSelect.cs            # Визуализация выбранного блока
├── Game1.cs               # Главный цикл игры
└── ...
```

**Правила**:
- Каждый класс — в отдельном файле с именем класса.
- Папки группируют логику по доменам (например, `World/`, `Utils/`).
- Ресурсы: сырые — в `Assets/`, обработанные — в `Content/`.


---

## 2. Именование


**Классы и методы**: `PascalCase`  
- `Camera`, `UpdateMatrices()`, `LoadTexture()`


**Поля и переменные**: `camelCase`  
- `spriteBatch`, `chunkSize`
- Приватные (и защищённые) поля класса пишутся с подчёркиванием: `_fieldName`.


**Константы**: `UPPER_CASE`  
- `MAX_CHUNK_SIZE`, `DEFAULT_FOV`


**Булевы переменные**: префикс `is`, `has`, `can`  
- `isLoaded`, `hasChanged`, `canRender`


**События**: суффикс `Changed`, `Updated`  
- `PositionChanged`, `ChunkUpdated`


---

## 3. Стиль кода


- **Отступы**: 4 пробела (не табуляция).
- **Длина строки**: до 100 символов (перенос при необходимости).
- **Скобки**: открывающая — на той же строке, что условие/метод.
  ```csharp
  if (condition) {
      // код
  }
  ```
- **Комментарии**: только на русском.
- **XML‑комментарии** для публичных методов/классов:
  ```csharp
  /// <summary>
  /// Загружает текстуру по пути.
  /// </summary>
  /// <param name="path">Путь к файлу (без расширения).</param>
  /// <returns>Текстура или null, если не найдена.</returns>
  public Texture2D LoadTexture(string path) { ... }
  ```

---

## 4. Работа с ресурсами (класс Res)

**Назначение**: централизованная загрузка и кэширование.


**Правила**:
- Все ресурсы загружаются через `Res.Instance.Load<T>(path)`.
- Кэширование включено по умолчанию (повторный вызов `Load` возвращает тот же объект).
- Для принудительной перезагрузки: `Res.Instance.Reload<T>(path)`.

- Пути указываются относительно `Content/` (без расширения):
  ```csharp
  var texture = Res.Instance.Load<Texture2D>("Blocks/Stone");
  ```

**Пример реализации Res.cs**:
```csharp
public class Res {
    private static Res _instance;
    public static Res Instance => _instance ??= new Res();


    private Dictionary<string, object> _cache = new();

    public T Load<T>(string path) where T : class {
        // Логика загрузки и кэширования
    }

    public void Reload<T>(string path) where T : class {
        // Логика перезагрузки
    }
}
```

---

## 5. Глобальное состояние (класс State)


**Назначение**: доступ к общим сервисам (без передачи параметров в конструкторы).


**Правила**:
- Все экземпляры хранятся как публичные статические свойства.
- Инициализация — в `Game1.Initialize()`.
- Запрещено изменять состояние извне без явного метода (например, `State.GraphicsDevice = null` — нельзя).


**Пример State.cs**:
```csharp
public class State {
    public static GraphicsDevice GraphicsDevice { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    public static Camera MainCamera { get; private set; }


    public static void Initialize(GraphicsDevice graphicsDevice) {
        GraphicsDevice = graphicsDevice;
        SpriteBatch = new SpriteBatch(graphicsDevice);
    }
}
```

---

## 6. Работа с MonoGame


- **GraphicsDevice**: всегда через `State.GraphicsDevice`.
- **SpriteBatch**: начало/конец в `Begin()`/`End()`:
  ```csharp
  State.SpriteBatch.Begin();
  // отрисовка
  State.SpriteBatch.End();
  ```
- **Матрицы**: камера пересчитывает `ViewMatrix` и `ProjectionMatrix` при изменении позиции/поворота.
- **Обновление**: логика в `Game1.Update()`, рендер — в `Game1.Draw()`.


---

## 7. Обработка ввода


- Все события мыши/клавиатуры — в `InputState` (отдельный класс).
- Проверка состояний: `InputState.IsKeyDown(Keys.W)`, `InputState.MouseLeftButtonDown()`.
- Делегирование действий в `Game1.Update()`:
  ```csharp
  if (InputState.IsKeyDown(Keys.W)) {
      State.MainCamera.MoveForward();
  }
  ```

---

## 8. Ошибки и исключения


- **Не критические ошибки** (например, отсутствующий ресурс): логирование через `Debug.WriteLine()`.
- **Критические ошибки**: `throw new InvalidOperationException("...")`.
- **Поведение по умолчанию**: не «проглатывать» исключения — передавать наверх.


---

## 9. Коммиты и Git


- **Сообщения коммитов**: формат `тип: описание` (англ.):
  - `feat: add Camera class`
  - `fix: handle null texture in Res`
  - `docs: update CONVENTIONS.md`
- **Ветки**: `main` — стабильное состояние, `feature/*` — эксперименты.
- **Push**: только после успешного локального теста.

---

## 10. Примечания для ассистента (меня)


При генерации кода учитывайте:
1. **Ваш стиль**: русский язык в комментариях, `camelCase` для полей.
2. **Архитектуру**: использование `Res` и `State` для ресурсов и глобальных ссылок.
3. **MonoGame**: совместимость с XNA/MonoGame API (например, `Matrix`, `GraphicsDevice`).
4. **Читаемость**: пояснения к нетривиальным участкам (например, «Пересчёт матрицы вида»).
5. **Тестирование**: предлагайте простые проверки (например, «Добавьте `Debug.WriteLine(position)` в Update()»).


---

### Как обновлять этот документ


1. При появлении нового правила — добавьте раздел.
2. При изменении соглашения — зачеркните старое (~~так~~), напишите новое.
3. В `NOTES.md` укажите:  
   ```markdown
   ## Сессия от 2025-10-31
   - Добавлено правило: работа с ресурсами через `Res.Instance`.
   - Изменено: стиль комментариев → только русский.
   ```

---

Теперь вы можете:
1. Скопировать этот шаблон в `CONVENTIONS.md`.
2. Отредактировать под свои нужды (убрать лишнее, добавить своё).
3. Закоммитить:  
   ```bash
   git add CONVENTIONS.md
   git commit -m "docs: update conventions with detailed rules"
   git push
   ```

После этого я буду строго следовать этим правилам в коде и объяснениях.

Если нужно что‑то уточнить или добавить — скажите!