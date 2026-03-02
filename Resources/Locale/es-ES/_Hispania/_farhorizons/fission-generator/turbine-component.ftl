# SPDX-FileCopyrightText: 2026 Hispania Station Contributors
#
# SPDX-License-Identifier: AGPL-3.0-or-later

### Examine

# Shown when examining the turbine.
turbine-spinning-0 = Las aspas no están girando.
turbine-spinning-1 = Las aspas giran lentamente.
turbine-spinning-2 = Las aspas están girando.
turbine-spinning-3 = Las aspas giran rápidamente.
turbine-spinning-4 = [color=red]¡Las aspas giran fuera de control![/color]

turbine-damaged-0 = Parece estar en buenas condiciones.
turbine-damaged-1 = La turbina parece un poco desgastada.
turbine-damaged-2 = [color=yellow]La turbina parece muy dañada.[/color]
turbine-damaged-3 = [color=orange]¡Está críticamente dañada![/color]

turbine-ruined = [color=red]¡Está completamente rota![/color]

### Popups

# Shown when an event occurs
turbine-overheat = ¡{$owner} activa la válvula de emergencia de sobrecalentamiento!
turbine-explode = ¡{$owner} se destroza a sí misma!

# Shown when damage occurs
turbine-spark = ¡{$owner} empieza a chispear!
turbine-spark-stop = {$owner} deja de chispear.
turbine-smoke = ¡{$owner} empieza a humear!
turbine-smoke-stop = {$owner} deja de humear.

# Shown during repairs
turbine-repair-ruined = Reparas la carcasa de {$target} con {$tool}.
turbine-repair = Reparas parte del daño de {$target} usando {$tool}.
turbine-no-damage = No hay daños que reparar en {$target} usando {$tool}.
turbine-show-damage = SaludAspa {$health}, SaludAspaMax {$healthMax}.

# Anchoring warnings
turbine-unanchor-warning = ¡No puedes desanclar la turbina de gas mientras está girando!
turbine-anchor-warning = Posición de anclaje inválida.

### UI

# Shown when using the UI
comp-turbine-ui-rpm = RPM

comp-turbine-ui-overspeed = SOBREVEL.
comp-turbine-ui-overtemp = SOBRETEMP.
comp-turbine-ui-stalling = CALADO
comp-turbine-ui-undertemp = BAJOTEMP.

comp-turbine-ui-flow-rate = Caudal
comp-turbine-ui-stator-load = Carga del Estátor

comp-turbine-ui-locked-message = Controles bloqueados.
comp-turbine-ui-footer-left = Peligro: maquinaria de movimiento rápido.
comp-turbine-ui-footer-right = 1.2 REV 1
