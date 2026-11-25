# TheLongDark Battery Power Mod

A lightweight gameplay mod for The Long Dark survival mode that allows players to use Car Batteries (GEAR_CarBattery) to power nearby lights and electrical fixtures when there is no active Aurora.
This mod introduces a dynamic charge and discharge system for car batteries, adding a practical use for them as backup power in your base.

<img width="1689" height="950" alt="image" src="https://github.com/user-attachments/assets/c13821a5-11b8-43d4-9dc5-0f534ddf36f8" />

| State | Condition Change Rate | Description |
| --- | --- | --- |
| Charging | +3% per hour | Recharges the battery when Aurora is active. |
| OnLoad | -1% per hour | Discharges the battery when there is no Aurora and nearby electrical fixtures exist. |
| UnLoad | -1% per day | Discharges the battery when there is no Aurora and no nearby electrical fixtures exist. |

Lighting control logic adapted from **[tld-house-lights](https://github.com/Xpazeman/tld-house-lights)** by **Xpazeman**.

Before installing this mod, ensure you have MelonLoader.
