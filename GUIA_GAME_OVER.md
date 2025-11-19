# Guía: Configurar Game Over en Unity

Esta guía te muestra cómo configurar el sistema de Game Over cuando el jugador muere.

## Scripts Creados

1. **GameOverManager.cs** - Controla la pantalla de Game Over
2. **PlayerHealth.cs** - Modificado para activar el Game Over al morir

---

## Configuración Paso a Paso

### Paso 1: Crear el Canvas de Game Over

1. **Click derecho en Hierarchy** > `UI > Canvas`
2. **Renombrar** el Canvas a `GameOverCanvas`
3. **En el Inspector del Canvas**:
   ```
   Canvas Scaler > UI Scale Mode: Scale With Screen Size
   Reference Resolution: 1920 x 1080
   ```

### Paso 2: Crear Panel de Fondo

1. **Click derecho en GameOverCanvas** > `UI > Panel`
2. **Renombrar** a `BackgroundPanel`
3. **Configurar**:
   ```
   Rect Transform: Stretch completo (Anchor Presets: Stretch/Stretch)
   Image > Color: Negro con Alpha 200 (para efecto semi-transparente)
   ```

### Paso 3: Crear Texto de "GAME OVER"

1. **Click derecho en GameOverCanvas** > `UI > Text` (o `TextMeshPro - Text` si tienes TMP)
2. **Renombrar** a `GameOverText`
3. **Configurar**:
   ```
   Rect Transform:
     - Anchor Presets: Center
     - Pos X: 0, Pos Y: 100
     - Width: 800, Height: 150

   Text:
     - Text: "GAME OVER"
     - Font Size: 80
     - Alignment: Center/Middle
     - Color: Rojo (255, 0, 0) o blanco
     - Font Style: Bold
   ```

### Paso 4: Crear Botones

#### Botón Restart

1. **Click derecho en GameOverCanvas** > `UI > Button`
2. **Renombrar** a `RestartButton`
3. **Configurar**:
   ```
   Rect Transform:
     - Anchor: Center
     - Pos X: 0, Pos Y: -50
     - Width: 250, Height: 60

   Texto del botón (hijo "Text"):
     - Text: "REINICIAR"
     - Font Size: 24
     - Color: Blanco
     - Alignment: Center/Middle
   ```

#### Botón Main Menu

1. **Click derecho en GameOverCanvas** > `UI > Button`
2. **Renombrar** a `MainMenuButton`
3. **Configurar**:
   ```
   Rect Transform:
     - Anchor: Center
     - Pos X: 0, Pos Y: -130
     - Width: 250, Height: 60

   Texto:
     - Text: "MENÚ PRINCIPAL"
     - Font Size: 24
   ```

#### Botón Quit (Opcional)

1. **Click derecho en GameOverCanvas** > `UI > Button`
2. **Renombrar** a `QuitButton`
3. **Configurar**:
   ```
   Rect Transform:
     - Anchor: Center
     - Pos X: 0, Pos Y: -210
     - Width: 250, Height: 60

   Texto:
     - Text: "SALIR"
     - Font Size: 24
   ```

### Paso 5: Crear GameObject del GameOverManager

1. **Click derecho en Hierarchy** > `Create Empty`
2. **Renombrar** a `GameOverManager`
3. **Agregar componente**: Buscar `GameOverManager` y agregarlo
4. **En el Inspector del GameOverManager**:
   ```
   Game Over Canvas: Arrastra "GameOverCanvas" aquí
   Game Over Text: Arrastra "GameOverText" aquí
   Restart Button: Arrastra "RestartButton" aquí
   Main Menu Button: Arrastra "MainMenuButton" aquí
   Quit Button: Arrastra "QuitButton" aquí (opcional)

   Main Menu Scene Name: "MainMenu" (o el nombre de tu escena de menú)
   Pause Game On Death: ✅
   Disable Player Controls: ✅
   ```

### Paso 6: Ocultar Canvas al Inicio

1. **Selecciona GameOverCanvas** en Hierarchy
2. **En el Inspector**, desactiva el checkbox al lado del nombre (arriba a la izquierda)
3. El Canvas debe estar **DESACTIVADO** al inicio

---

## Configuración Opcional: Efectos Visuales

### Agregar Animación al Texto

1. **Selecciona GameOverText**
2. **Add Component** > `Animator`
3. Crear animación de fade in o scale up (opcional)

### Agregar Sonido de Game Over

1. **Selecciona GameOverManager**
2. **En el Inspector**:
   ```
   Game Over Sound: Arrastra tu AudioClip de Game Over
   ```

---

## Testing

### Probar el Game Over

1. **Dale Play ▶️**
2. **En la Console**, escribe esto para probar:
   ```csharp
   // Encontrar el player y hacerle daño
   GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(100);
   ```

O simplemente **deja que un zombie te mate**.

### Verificar Funcionalidad

- ✅ El Canvas aparece cuando mueres
- ✅ El juego se pausa (zombies dejan de moverse)
- ✅ El cursor se hace visible
- ✅ Botón "Reiniciar" recarga la escena
- ✅ Botón "Menú" lleva al menú principal
- ✅ Botón "Salir" cierra el juego

---

## Estructura Final en Hierarchy

```
Hierarchy:
├─ GameOverManager (con script GameOverManager)
├─ GameOverCanvas (Canvas)
│  ├─ BackgroundPanel (Panel negro semi-transparente)
│  ├─ GameOverText (Texto "GAME OVER")
│  ├─ RestartButton (Botón "REINICIAR")
│  ├─ MainMenuButton (Botón "MENÚ PRINCIPAL")
│  └─ QuitButton (Botón "SALIR")
├─ PlayerFPS (con script PlayerHealth)
└─ ... (resto de objetos)
```

---

## Solución de Problemas

### El Game Over no aparece

- ✅ Verifica que `GameOverCanvas` esté asignado en `GameOverManager`
- ✅ Verifica que existe un `GameOverManager` en la escena
- ✅ Revisa la Console para ver el mensaje "💀 PLAYER DEAD"

### Los botones no funcionan

- ✅ Verifica que los botones estén asignados en `GameOverManager`
- ✅ Asegúrate de que los botones tienen componente `Button`
- ✅ Verifica que hay un `EventSystem` en la escena (se crea automáticamente con el Canvas)

### El juego no se pausa

- ✅ Verifica que `Pause Game On Death: ✅` en GameOverManager
- ✅ Algunos sistemas ignoran `Time.timeScale`, usa `Disable Player Controls` también

### El cursor no aparece

- ✅ Asegúrate de que no hay otro script bloqueando el cursor
- ✅ Verifica en el código que `Cursor.visible = true` se ejecuta

---

## Personalización

### Cambiar Colores

**Fondo del panel:**
```
BackgroundPanel > Image > Color: Ajusta RGB y Alpha
```

**Botones:**
```
RestartButton > Button > Colors:
  - Normal Color: Color normal
  - Highlighted Color: Color al pasar mouse
  - Pressed Color: Color al clickear
```

### Agregar Estadísticas

Puedes agregar más textos para mostrar:
- Zombies eliminados
- Tiempo sobrevivido
- Puntuación

```csharp
// En GameOverManager.cs, agregar:
public Text statsText;

public void ShowGameOver()
{
    // ... código existente ...

    if (statsText != null)
    {
        statsText.text = $"Zombies eliminados: {zombieKillCount}\nTiempo: {survivalTime}s";
    }
}
```

---

## Integración con Build Settings

Para que el botón "Main Menu" funcione en build:

1. **File > Build Settings**
2. **Agregar escenas**:
   - Escena 0: MainMenu (o tu menú principal)
   - Escena 1: World_2_Asylum (nivel de juego)
3. **En GameOverManager**:
   ```
   Main Menu Scene Name: "MainMenu"
   ```

---

## Archivos Modificados/Creados

- ✅ `Assets/Scripts/GameOverManager.cs` - Nuevo script
- ✅ `Assets/Scripts/PlayerHealth.cs` - Modificado para llamar Game Over
- ✅ `GUIA_GAME_OVER.md` - Esta guía

---

**¡Listo! Ahora tu juego muestra Game Over cuando el jugador muere!** 💀🎮

## Próximos Pasos (Opcional)

- Agregar animaciones de fade in/out
- Agregar efectos de sonido
- Agregar pantalla de victoria cuando completes niveles
- Guardar high scores o estadísticas
