# SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 Kira Bridgeton <161087999+Verbalase@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 KrasnoshchekovPavel <119816022+KrasnoshchekovPavel@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 Steve <marlumpy@gmail.com>
# SPDX-FileCopyrightText: 2024 icekot8 <93311212+icekot8@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 marc-pelletier <113944176+marc-pelletier@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 potato1234_x <79580518+potato1234x@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later

reagent-effect-condition-guidebook-total-damage =
    { $max ->
        [2147483648] tiene al menos {NATURALFIXED($min, 2)} de daño total
        *[other] { $min ->
                    [0] tiene como máximo {NATURALFIXED($max, 2)} de daño total
                    *[other] tiene entre {NATURALFIXED($min, 2)} y {NATURALFIXED($max, 2)} de daño total
                 }
    }

reagent-effect-condition-guidebook-total-hunger =
    { $max ->
        [2147483648] el objetivo tiene al menos {NATURALFIXED($min, 2)} de hambre total
        *[other] { $min ->
                    [0] el objetivo tiene como máximo {NATURALFIXED($max, 2)} de hambre total
                    *[other] el objetivo tiene entre {NATURALFIXED($min, 2)} y {NATURALFIXED($max, 2)} de hambre total
                 }
    }

reagent-effect-condition-guidebook-reagent-threshold =
    { $max ->
        [2147483648] hay al menos {NATURALFIXED($min, 2)}u de {$reagent}
        *[other] { $min ->
                    [0] hay como máximo {NATURALFIXED($max, 2)}u de {$reagent}
                    *[other] hay entre {NATURALFIXED($min, 2)}u y {NATURALFIXED($max, 2)}u de {$reagent}
                 }
    }

reagent-effect-condition-guidebook-mob-state-condition =
    la entidad está { $state }

reagent-effect-condition-guidebook-job-condition =
    el trabajo del objetivo es { $job }

reagent-effect-condition-guidebook-solution-temperature =
    la temperatura de la solución es { $max ->
            [2147483648] al menos {NATURALFIXED($min, 2)}k
            *[other] { $min ->
                        [0] como máximo {NATURALFIXED($max, 2)}k
                        *[other] entre {NATURALFIXED($min, 2)}k y {NATURALFIXED($max, 2)}k
                     }
    }

reagent-effect-condition-guidebook-body-temperature =
    la temperatura del cuerpo es { $max ->
            [2147483648] al menos {NATURALFIXED($min, 2)}k
            *[other] { $min ->
                        [0] como máximo {NATURALFIXED($max, 2)}k
                        *[other] entre {NATURALFIXED($min, 2)}k y {NATURALFIXED($max, 2)}k
                     }
    }

reagent-effect-condition-guidebook-organ-type =
    el órgano metabolizador { $shouldhave ->
                                [true] es
                                *[false] no es
                           } un órgano de tipo {$name}

reagent-effect-condition-guidebook-has-tag =
    el objetivo { $invert ->
                 [true] no tiene
                 *[false] tiene
                } la etiqueta {$tag}

reagent-effect-condition-guidebook-blood-reagent-threshold =
    { $max ->
        [2147483648] hay al menos {NATURALFIXED($min, 2)}u de {$reagent}
        *[other] { $min ->
                    [0] hay como máximo {NATURALFIXED($max, 2)}u de {$reagent}
                    *[other] hay entre {NATURALFIXED($min, 2)}u y {NATURALFIXED($max, 2)}u de {$reagent}
                 }
    }

reagent-effect-condition-guidebook-this-reagent = este reactivo

reagent-effect-condition-guidebook-breathing =
    el metabolizador está { $isBreathing ->
                [true] respirando normalmente
                *[false] asfixiándose
               }

reagent-effect-condition-guidebook-internals =
    el metabolizador está { $usingInternals ->
                [true] usando oxígeno interno
                *[false] respirando aire atmosférico
               }
