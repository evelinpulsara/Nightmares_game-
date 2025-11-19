# Solución: Zombie se Queda Atascado en la Pared

## Problema
El zombie no sigue el camino y se buggea caminando contra la pared.

## Causas Comunes
1. **NavMesh mal configurado** - El NavMesh no cubre toda el área caminable
2. **Parámetros del NavMeshAgent** - Radio, altura o velocidad incorrectos
3. **Obstáculos no configurados** - Paredes sin NavMeshObstacle
4. **Colisiones conflictivas** - Colliders interfiriendo con el NavMeshAgent

---

## Soluciones Rápidas

### Solución 1: Usar el Script ZombieNavMeshFixer (RECOMENDADO)

1. **Selecciona el prefab del Zombie**
2. **Agrega el componente** `ZombieNavMeshFixer`
3. **Configura** (valores por defecto están bien):
   ```
   Stuck Time: 2
   Min Movement Speed: 0.1
   Unstuck Distance: 3
   Show Debug: ✅ (para ver qué pasa)
   Draw Path: ✅ (para ver la ruta)
   ```

**Esto automáticamente**:
- Detecta cuando el zombie está atascado
- Lo mueve a una posición válida
- Recalcula la ruta si es necesaria
- Optimiza parámetros del NavMeshAgent

---

### Solución 2: Rehacer el NavMesh

1. **Window > AI > Navigation**
2. **Pestaña "Bake"**
3. **Configura**:
   ```
   Agent Radius: 0.3 - 0.5
   Agent Height: 2.0
   Max Slope: 45
   Step Height: 0.4
   ```
4. **Click en "Bake"**

⚠️ **Importante**: Asegúrate de que el NavMesh cubre TODO el piso donde el zombie debe caminar.

---

### Solución 3: Ajustar NavMeshAgent del Zombie

Si NO quieres usar el script nuevo, ajusta manualmente:

1. **Selecciona el Zombie**
2. **Componente NavMeshAgent**:
   ```
   Speed: 3.5
   Angular Speed: 120
   Acceleration: 8
   Stopping Distance: 1.2
   Auto Braking: ✅
   Radius: 0.3 - 0.5
   Height: 2.0
   Obstacle Avoidance: High Quality
   Avoidance Priority: 50
   Auto Traverse Off Mesh Link: ✅
   Auto Repath: ✅
   ```

---

### Solución 4: Verificar Colisiones

1. **Selecciona el Zombie**
2. **Verifica que tiene**:
   - `Capsule Collider` (NO Box Collider)
   - Radius: 0.3 - 0.5
   - Height: 2.0
   - Center: (0, 1, 0)

3. **Rigidbody configurado como**:
   ```
   Is Kinematic: ✅
   Use Gravity: ❌
   Constraints: Freeze Rotation X, Y, Z ✅
   ```

---

### Solución 5: Marcar Paredes como Obstáculos

1. **Selecciona las paredes**
2. **Agrega componente** `NavMesh Obstacle`
3. **Configura**:
   ```
   Shape: Box
   Carve: ✅
   ```

---

## Diagnóstico: Cómo Saber Cuál es el Problema

### A. Ver el NavMesh en Unity
1. **Window > AI > Navigation**
2. En la **Scene view**, verás áreas azules
3. **Las áreas azules** = donde el zombie puede caminar
4. **Si hay huecos** = problema del NavMesh

### B. Ver la Ruta del Zombie
1. Agrega `ZombieNavMeshFixer` con `Draw Path: ✅`
2. Selecciona el zombie en Play mode
3. Verás una **línea amarilla** = su ruta
4. Si la ruta va **directo a la pared** = problema del NavMesh

### C. Ver Mensajes de Debug
1. Activa `Show Debug: ✅` en `ZombieNavMeshFixer`
2. Abre la **Console** (Ctrl/Cmd + Shift + C)
3. Verás mensajes como:
   - 🧟 "Zombie atascado" = problema detectado
   - ✅ "Zombie movido" = solución aplicada
   - ❌ "No se encontró posición válida" = NavMesh muy malo

---

## Solución Paso a Paso Completa

### Paso 1: Limpiar el Proyecto
```
1. Window > AI > Navigation > Bake > Clear
2. Espera a que se limpie
```

### Paso 2: Rehacer NavMesh
```
1. Selecciona TODOS los pisos donde el zombie debe caminar
2. Inspector > Navigation (arriba a la derecha) > ✅ Navigation Static
3. Window > AI > Navigation > Bake
4. Ajusta Agent Radius: 0.4, Height: 2.0
5. Click "Bake"
```

### Paso 3: Configurar Zombie
```
1. Selecciona el prefab del Zombie
2. Agrega ZombieNavMeshFixer
3. Verifica NavMeshAgent:
   - Speed: 3.5
   - Radius: 0.4
   - Height: 2.0
   - Obstacle Avoidance: High Quality
```

### Paso 4: Probar
```
1. Play ▶️
2. Observa si el zombie sigue al jugador
3. Si se ataca, revisa Console para ver por qué
```

---

## Configuración Recomendada Final

### Para el Zombie (Prefab):
```
Componentes:
├─ EnemyZombi (script existente)
├─ ZombieNavMeshFixer (NUEVO - arregla bugs)
├─ NavMeshAgent
│  ├─ Speed: 3.5
│  ├─ Radius: 0.4
│  ├─ Height: 2.0
│  └─ Obstacle Avoidance: High Quality
├─ Capsule Collider
│  ├─ Radius: 0.3
│  ├─ Height: 2.0
│  └─ Center: (0, 1, 0)
└─ Rigidbody
   ├─ Is Kinematic: ✅
   └─ Constraints: Freeze Rotation
```

### Para el NavMesh (Escena):
```
Bake Settings:
├─ Agent Radius: 0.4
├─ Agent Height: 2.0
├─ Max Slope: 45
├─ Step Height: 0.4
└─ Drop Height: Unlimited
```

---

## Problemas Comunes y Soluciones

### "El zombie camina en el aire"
- **Causa**: NavMesh mal horneado
- **Solución**: Rebakear NavMesh, verificar que los pisos son Navigation Static

### "El zombie atraviesa paredes"
- **Causa**: Paredes no tienen NavMeshObstacle
- **Solución**: Agregar NavMesh Obstacle a las paredes con Carve ✅

### "El zombie se teletransporta"
- **Causa**: NavMesh con huecos
- **Solución**: Rebakear NavMesh, reducir Agent Radius

### "El zombie no encuentra al jugador"
- **Causa**: NavMesh no conecta las áreas
- **Solución**: Verificar que todo el área sea Navigation Static y rebakear

### "El zombie es muy lento"
- **Causa**: Speed muy bajo
- **Solución**: NavMeshAgent > Speed: 3.5 o más

---

## Testing Checklist

Antes de dar por resuelto:

- [ ] El zombie camina hacia el jugador
- [ ] El zombie rodea obstáculos (no camina directo a la pared)
- [ ] El zombie puede seguir al jugador a diferentes habitaciones
- [ ] No hay mensajes de error en Console
- [ ] La ruta (línea amarilla) se ve correcta

---

## Archivos Creados

- `Assets/Scripts/ZombieNavMeshFixer.cs` - Script automático de solución
- `SOLUCION_ZOMBIE_BUGGEADO.md` - Esta guía

---

**¡Con estos pasos tu zombie debería seguir al jugador correctamente!** 🧟‍♂️➡️🏃
