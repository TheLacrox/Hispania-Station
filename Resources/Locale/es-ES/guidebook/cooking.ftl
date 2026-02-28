# SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 Aiden <aiden@djkraz.com>
# SPDX-FileCopyrightText: 2025 pathetic meowmeow <uhhadd@gmail.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later

guidebook-microwave-ingredients-header = Ingredientes
guidebook-microwave-cook-time-header = Tiempo de Cocción
guidebook-microwave-cook-time =
    { $time ->
        [0] Instantáneo
        [1] [bold]1[/bold] segundo
       *[other] [bold]{$time}[/bold] segundos
    }

guidebook-microwave-reagent-color-display = [color={$color}]■[/color]
guidebook-microwave-reagent-name-display = [bold]{$reagent}[/bold]
guidebook-microwave-reagent-quantity-display = × {$amount}u

guidebook-microwave-solid-name-display = [bold]{$ingredient}[/bold]
guidebook-microwave-solid-quantity-display = × {$amount}
