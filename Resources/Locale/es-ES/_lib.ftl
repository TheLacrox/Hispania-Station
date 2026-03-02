# SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
# SPDX-FileCopyrightText: 2021 mirrorcult <notzombiedude@gmail.com>
# SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
# SPDX-FileCopyrightText: 2023 Repo <47093363+Titian3@users.noreply.github.com>
# SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 2024 Stalen <33173619+stalengd@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 Aiden <28298836+Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 2025 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-3.0-or-later

### Mensajes especiales usados por el localizador interno.

# Usado internamente por la función PRESSURE().
zzzz-fmt-pressure = { TOSTRING($divided, "F1") } { $places ->
    [0] kPa
    [1] MPa
    [2] GPa
    [3] TPa
    [4] PBa
    *[5] ???
}

# Usado internamente por la función POWERWATTS().
zzzz-fmt-power-watts = { TOSTRING($divided, "F1") } { $places ->
    [0] W
    [1] kW
    [2] MW
    [3] GW
    [4] TW
    *[5] ???
}

# Usado internamente por la función POWERJOULES().
# Recordatorio: 1 julio = 1 vatio durante 1 segundo (multiplicar vatios por segundos para obtener julios).
# Por tanto, 1 kilovatio-hora equivale a 3.600.000 julios (3,6 MJ)
zzzz-fmt-power-joules = { TOSTRING($divided, "F1") } { $places ->
    [0] J
    [1] kJ
    [2] MJ
    [3] GJ
    [4] TJ
    *[5] ???
}

# Usado internamente por la función ENERGYWATTHOURS().
zzzz-fmt-energy-watt-hours = { TOSTRING($divided, "F1") } { $places ->
    [0] Wh
    [1] kWh
    [2] MWh
    [3] GWh
    [4] TWh
    *[5] ???
}

# Usado internamente por la función PLAYTIME().
zzzz-fmt-playtime = {$hours}H {$minutes}M

# Usado internamente por la función THE().
zzzz-the = { $ent }

# Usado internamente por la función SUBJECT().
zzzz-subject-pronoun = { GENDER($ent) ->
    [male] él
    [female] ella
    [epicene] elles
   *[neuter] ello
   }

# Usado internamente por la función OBJECT().
zzzz-object-pronoun = { GENDER($ent) ->
    [male] lo
    [female] la
    [epicene] les
   *[neuter] lo
   }

# Usado internamente por la función DAT-OBJ().
zzzz-dat-object = { GENDER($ent) ->
    [male] le
    [female] le
    [epicene] les
   *[neuter] le
   }

# Usado internamente por la función GENITIVE().
zzzz-genitive = { GENDER($ent) ->
    [male] su
    [female] su
    [epicene] su
   *[neuter] su
   }

# Usado internamente por la función POSS-PRONOUN().
zzzz-possessive-pronoun = { GENDER($ent) ->
    [male] suyo
    [female] suya
    [epicene] suyos
   *[neuter] suyo
   }

# Usado internamente por la función POSS-ADJ().
zzzz-possessive-adjective = { GENDER($ent) ->
    [male] su
    [female] su
    [epicene] su
   *[neuter] su
   }

# Usado internamente por la función REFLEXIVE().
zzzz-reflexive-pronoun = { GENDER($ent) ->
    [male] sí mismo
    [female] sí misma
    [epicene] sí mismos
   *[neuter] sí mismo
   }

# Usado internamente por la función CONJUGATE-BE().
zzzz-conjugate-be = { GENDER($ent) ->
    [epicene] son
   *[other] es
   }

# Usado internamente por la función CONJUGATE-HAVE().
zzzz-conjugate-have = { GENDER($ent) ->
    [epicene] tienen
   *[other] tiene
   }

# Usado internamente por la función CONJUGATE-BASIC().
zzzz-conjugate-basic = { GENDER($ent) ->
    [epicene] { $first }
   *[other] { $second }
   }
