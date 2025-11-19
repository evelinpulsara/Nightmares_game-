# Solución: Zombies Empujan al Jugador

## Problema
Cuando los zombies se acercan, empujan al jugador y lo mueven de forma incontrolada.

## Causa
Conflicto de física entre:
- **NavMeshAgent** del zombie (intenta moverse hacia el jugador)
- **CharacterController** o **Rigidbody** del jugador
- **Colliders** que chocan entre sí

---

## SOLUCIÓN RÁPIDA (Recomendada)

### Opción 1: Configurar Layers de Física

#### Paso 1: Crear Layer para Zombies
1. **Selecciona cualquier zombie** en Hierarchy
2. **En Inspector**, arriba verás **Layer** > Click **"Add Layer..."**
3. En **User Layer 6**, escribe: `Enemy`
4. **Vuelve a seleccionar el zombie**
5. **Layer** > Selecciona `Enemy`
6. Cuando pregunte "Change children?", click **"Yes, change children"**
7. **Repite para TODOS los zombies** (o hazlo en el prefab)

#### Paso 2: Configurar Physics Layers
1. **Edit > Project Settings**
2. **Physics** (en el menú izquierdo)
3. Baja hasta **Layer Collision Matrix**
4. **Busca la fila "Enemy"** y la columna **"Player"**
5. **DESMARCA** el checkbox donde se cruzan
   - Esto hace que Enemy y Player NO colisionen físicamente

#### Paso 3: Verificar
1. Dale Play ▶️
2. Los zombies deben acercarse pero **NO empujarte**
3. Pueden atacarte pero no moverte

---

## SOLUCIÓN ALTERNATIVA 1: Usar Script ZombiePushFix

Si no quieres tocar layers manualmente:

### Paso 1: Agregar Script al Zombie
1. **Selecciona el prefab del zombie**
2. **Add Component** > Buscar `ZombiePushFix`
3. El script se auto-configura

### Configuración en Inspector:
```
Player Layer: "Player"
Disable Rigidbody Near Player: ✅
Player Detection Range: 3
```

---

## SOLUCIÓN ALTERNATIVA 2: Ajustar NavMeshAgent

### Para cada Zombie:

1. **Selecciona el zombie**
2. **NavMeshAgent** en Inspector:
   ```
   Avoidance Priority: 90  (mayor = menos prioridad)
   Radius: 0.5  (ajustar al tamaño del zombie)
   Obstacle Avoidance Type: High Quality
   ```

### Para el Jugador:

Si el jugador también tiene NavMeshAgent (NO debería):
```
Avoidance Priority: 10  (menor = más prioridad)
```

---

## SOLUCIÓN ALTERNATIVA 3: Trigger Collider para Ataque

Crear un collider separado solo para detectar ataques:

### Paso 1: Modificar Collider Principal
1. **Selecciona el zombie**
2. **Capsule Collider**:
   ```
   Is Trigger: ❌ (NO trigger)
   ```

### Paso 2: Crear Collider de Ataque
1. **Click derecho en el zombie** > Create Empty
2. **Renombrar** a `AttackTrigger`
3. **Add Component** > `Sphere Collider`
4. **Configurar**:
   ```
   Is Trigger: ✅
   Radius: 2 (rango de ataque)
   ```

### Paso 3: Script de Ataque
Modificar `EnemyZombi.cs` para usar `OnTriggerStay` en lugar de distancia:

```csharp
void OnTriggerStay(Collider other)
{
    if (other.CompareTag("Player"))
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }
    }
}
```

---

## CONFIGURACIÓN COMPLETA RECOMENDADA

### Para el Zombie:

```
Layer: Enemy

Rigidbody:
  Is Kinematic: ✅
  Use Gravity: ❌
  Constraints: Freeze Rotation X, Y, Z

NavMeshAgent:
  Avoidance Priority: 90
  Radius: 0.4
  Height: 2.0
  Obstacle Avoidance: High Quality

Capsule Collider:
  Is Trigger: ❌
  Radius: 0.3
  Height: 2.0
```

### Para el Jugador:

```
Layer: Player

Character Controller:
  Radius: 0.3
  Height: 2.0
  Center: (0, 1, 0)

Rigidbody: ❌ NO debe tener (si usa CharacterController)
```

### Physics Settings:

```
Edit > Project Settings > Physics
Layer Collision Matrix:
  Enemy ↔ Player: ❌ DESMARCADO
```

---

## Testing Checklist

Después de aplicar la solución:

- [ ] Los zombies se acercan sin empujar
- [ ] El jugador mantiene control de movimiento
- [ ] Los zombies pueden atacar normalmente
- [ ] Los zombies colisionan con paredes/obstáculos
- [ ] El jugador no atraviesa zombies (opcional)

---

## Problemas Comunes

### "Los zombies atraviesan las paredes"
- **Solución**: Solo desactiva colisión entre Enemy y Player, NO entre Enemy y Default

### "El jugador puede atravesar zombies"
- Esto es NORMAL si desactivaste la colisión
- Los zombies aún pueden atacarte
- Si quieres bloqueo parcial, usa Avoidance Priority en lugar de desactivar colisión

### "Los zombies se empujan entre ellos"
- **Solución**: Variar `Avoidance Priority` entre 50-90 para cada zombie
- En `ZombieNavMeshFixer.cs` ya hace esto automáticamente

---

## Resumen de Métodos

| Método | Dificultad | Efectividad | Recomendado |
|--------|-----------|-------------|-------------|
| Layer Collision Matrix | Fácil | ⭐⭐⭐⭐⭐ | ✅ SÍ |
| ZombiePushFix Script | Media | ⭐⭐⭐⭐ | ✅ SÍ |
| Ajustar NavMeshAgent | Fácil | ⭐⭐⭐ | Complemento |
| Trigger Collider | Difícil | ⭐⭐⭐⭐ | Avanzado |

---

**Recomendación Final:**

**Usa la Opción 1 (Layer Collision Matrix)** - Es la más simple y efectiva.

1. Crear layer "Enemy"
2. Asignarlo a zombies
3. Desactivar colisión Enemy ↔ Player en Physics Settings

¡Listo en 2 minutos!
