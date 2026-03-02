# SPDX-FileCopyrightText: 2026 Capibara Station Contributors
#
# SPDX-License-Identifier: AGPL-3.0-or-later

### Popups
reactor-smoke-start = ¡{$owner} empieza a humear!
reactor-smoke-stop = {$owner} deja de humear.
reactor-fire-start = ¡{$owner} empieza a arder!
reactor-fire-stop = {$owner} deja de arder.

reactor-unanchor-warning = ¡No puedes desanclar el reactor nuclear mientras no esté vacío o tenga más de 80°C!
reactor-anchor-warning = Posición de anclaje inválida.

### Messages
reactor-smoke-start-message = ALERTA: {$owner} ha alcanzado una temperatura peligrosa: {$temperature}K. Intervenga inmediatamente para prevenir una fusión.
reactor-smoke-stop-message = {$owner} se ha enfriado por debajo de la temperatura peligrosa. Que tenga un buen día.
reactor-fire-start-message = ALERTA: {$owner} ha alcanzado temperatura CRÍTICA: {$temperature}K. FUSIÓN INMINENTE.
reactor-fire-stop-message = {$owner} se ha enfriado por debajo de la temperatura crítica. Fusión evitada.

reactor-temperature-dangerous-message = {$owner} está a temperatura peligrosa: {$temperature}K.
reactor-temperature-critical-message = {$owner} está a temperatura crítica: {$temperature}K.
reactor-temperature-cooling-message = {$owner} se está enfriando: {$temperature}K.

reactor-melting-announcement = Un reactor nuclear a bordo de la estación está comenzando a fundirse. Se recomienda evacuar el área circundante.
reactor-melting-announcement-sender = Emergencia Nuclear

reactor-meltdown-announcement = Un reactor nuclear a bordo de la estación ha sufrido una sobrecarga catastrófica. Es probable que haya escombros radiactivos, lluvia radiactiva e incendios de refrigerante. Se recomienda encarecidamente la evacuación inmediata del área circundante.
reactor-meltdown-announcement-sender = Fusión Nuclear

### UI
comp-nuclear-reactor-ui-locked = Bloqueado
comp-nuclear-reactor-ui-insert-button = Insertar
comp-nuclear-reactor-ui-remove-button = Retirar
comp-nuclear-reactor-ui-eject-button = Expulsar

comp-nuclear-reactor-ui-view-change = Cambiar Vista
comp-nuclear-reactor-ui-view-temp = Vista de Temperatura
comp-nuclear-reactor-ui-view-neutron = Vista de Neutrones
comp-nuclear-reactor-ui-view-target = Vista de Objetivo

comp-nuclear-reactor-ui-status-panel = Estado del Reactor
comp-nuclear-reactor-ui-reactor-temp = Temperatura
comp-nuclear-reactor-ui-reactor-rads = Radiación
comp-nuclear-reactor-ui-reactor-therm = Potencia Térmica
comp-nuclear-reactor-ui-reactor-control = Barras de Control
comp-nuclear-reactor-ui-therm-format = { POWERWATTS($power) }

comp-nuclear-reactor-ui-footer-left = Peligro: alta radiación.
comp-nuclear-reactor-ui-footer-right = 0.7 REV 1
