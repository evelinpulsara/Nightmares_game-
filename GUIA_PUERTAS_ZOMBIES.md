# Guía de Puertas que Zombies Pueden Abrir

Esta guía explica cómo configurar puertas que se abren automáticamente cuando los zombies se acercan, dando la ilusión de que los zombies las están abriendo o rompiendo.

## Scripts Disponibles

### 1. AutoDoorOpener.cs
**Apertura automática y silenciosa**
- La puerta se abre automáticamente cuando zombies están cerca
- Ideal para puertas que "se abren solas"
- Se cierra automáticamente después de un tiempo

### 2. ZombieDoorBreaker.cs
**Zombies rompiendo puertas con efectos**
- Los zombies golpean la puerta repetidamente
- Sonidos de golpes y sacudidas
- La puerta se rompe después de X golpes
- Más dramático e inmersivo

---

## Configuración Básica

### Opción 1: Puerta que se Abre Automáticamente (Simple)

1. **Selecciona el GameObject de la puerta**
2. **Agrega el componente** `AutoDoorOpener`
3. **Configura los parámetros**:

```
Enemy Tag: "Enemy"               // Tag de los zombies
Detection Radius: 3              // Distancia de activación
Open Duration: 5                 // Segundos abierta
Auto Close: ✅                   // Cierra automáticamente
Use Manual Rotation: ✅          // Usar rotación
Open Angle: 90                   // Grados de apertura
Rotation Speed: 90               // Velocidad de rotación
```

4. **Configura el Trigger (IMPORTANTE)**:
   - La puerta necesita un **Collider** para detectar zombies
   - Puedes usar el collider existente o agregar uno nuevo

---

### Opción 2: Zombies Rompiendo Puertas (Avanzado)

1. **Selecciona el GameObject de la puerta**
2. **Agrega el componente** `ZombieDoorBreaker`
3. **Configura los parámetros**:

```
Enemy Tag: "Enemy"
Knock Range: 2                   // Distancia para golpear
Hits To Break: 5                 // Golpes necesarios
Time Between Knocks: 1.5         // Tiempo entre golpes
Can Break: ✅                    // ¿Puede romperse?
Only Open: ❌                    // Solo abrir (no romper)
```

4. **Agrega efectos de audio** (opcional):
   - Arrastra clips de audio a `Knock Sounds` (golpes)
   - Arrastra clip a `Break Sound` (rotura)

5. **Agrega efectos visuales** (opcional):
   - Arrastra prefabs de partículas a `Hit Particles`
   - Arrastra prefabs de partículas a `Break Particles`

---

## Configuración Combinada (Recomendado)

Para máximo realismo, usa **ambos scripts juntos**:

### Configuración Paso a Paso

1. **Agrega `AutoDoorOpener`**:
   ```
   Detection Radius: 3
   Auto Close: ✅
   Open Angle: 90
   ```

2. **Agrega `ZombieDoorBreaker`**:
   ```
   Hits To Break: 5
   Only Open: ✅              // Solo abre, no destruye
   Knock Range: 2
   ```

**Resultado**: Los zombies golpean la puerta, hace ruido y se sacude, y después de 5 golpes se abre automáticamente.

---

## Ejemplos de Configuración

### Puerta Débil (Se Abre Rápido)
```
ZombieDoorBreaker:
  Hits To Break: 3
  Time Between Knocks: 1.0
  Only Open: ✅
```

### Puerta Fuerte (Se Rompe)
```
ZombieDoorBreaker:
  Hits To Break: 10
  Time Between Knocks: 2.0
  Can Break: ✅
  Only Open: ❌
```

### Puerta Automática (Sin Golpes)
```
AutoDoorOpener:
  Detection Radius: 4
  Auto Close: ✅
  Open Duration: 8
```

---

## Integración con DoorController

Si ya tienes el script `DoorController` en tu puerta:

1. Los scripts nuevos lo detectarán automáticamente
2. Se integrarán con el sistema existente
3. Funcionarán juntos sin conflictos

**Ejemplo de GameObject completo**:
```
GameObject: Puerta_Principal
├─ DoorController          // Para que el jugador la abra
├─ AutoDoorOpener          // Para que zombies la abran
└─ ZombieDoorBreaker       // Para efectos de golpes
```

---

## Consejos y Trucos

### Para Mejor Rendimiento
- Usa `Detection Radius` pequeño (2-3 metros)
- No uses demasiadas puertas con `ZombieDoorBreaker` simultáneamente

### Para Más Realismo
- Agrega sonidos de madera crujiendo
- Usa partículas de polvo/astillas
- Configura `Shake Intensity` a 0.1-0.3

### Para Horror/Tensión
- Aumenta `Hits To Break` a 8-10
- Reduce `Time Between Knocks` a 1.0
- Activa sonidos fuertes de golpes

---

## Solución de Problemas

### La puerta no se abre
- ✅ Verifica que los zombies tengan el tag "Enemy"
- ✅ Verifica que `Detection Radius` sea suficiente
- ✅ Verifica que la puerta tenga collider

### La puerta se abre para el jugador también
- Esto es normal si usas `AutoDoorOpener`
- Para evitarlo, usa solo `ZombieDoorBreaker` con `Only Open: ✅`

### Los golpes no suenan
- ✅ Verifica que tengas AudioClips asignados
- ✅ Verifica que el GameObject tenga AudioSource
- ✅ Verifica el volumen del audio

### La puerta no se rompe
- ✅ Verifica que `Can Break: ✅`
- ✅ Verifica que `Only Open: ❌`
- ✅ Asegúrate de que llegue a `Hits To Break`

---

## Configuración Recomendada por Escenario

### Hospital/Interior
```
AutoDoorOpener + ZombieDoorBreaker
Detection Radius: 2.5
Hits To Break: 5
Only Open: ✅
```

### Exterior/Barricada
```
Solo ZombieDoorBreaker
Hits To Break: 10
Can Break: ✅
Knock Sounds: Golpes fuertes
```

### Área Segura
```
Solo DoorController
(No usar AutoDoorOpener/ZombieDoorBreaker)
```

---

## Archivos Creados

- `Assets/Scripts/AutoDoorOpener.cs` - Apertura automática
- `Assets/Scripts/ZombieDoorBreaker.cs` - Sistema de golpes
- `Assets/Scripts/DoorController.cs` - Control manual (existente)

---

**¡Listo! Ahora tus zombies pueden abrir y romper puertas de forma realista!** 🧟🚪💥
