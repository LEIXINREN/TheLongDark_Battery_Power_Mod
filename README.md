# TheLongDark Battery Power Mod

A lightweight gameplay mod for The Long Dark survival mode that allows players to use Car Batteries (GEAR_CarBattery) to power nearby lights and electrical fixtures when there is no active Aurora.
This mod introduces a dynamic charge and discharge system for car batteries, adding a practical use for them as backup power in your base.

<img width="2730" height="1536" alt="pic" src="https://github.com/user-attachments/assets/26bd7ddd-c0df-40a7-98d4-24a92a744841" />

| State | Condition Change Rate | Description |
| --- | --- | --- |
| Charging | +3% per hour | Recharges the battery when Aurora is active. |
| OnLoad | -1% per hour | Discharges the battery when there is no Aurora and nearby electrical fixtures exist. |
| UnLoad | -1% per day | Discharges the battery when there is no Aurora and no nearby electrical fixtures exist. |

Lighting control logic adapted from **[tld-house-lights](https://github.com/Xpazeman/tld-house-lights)** by **Xpazeman**.

Before installing this mod, ensure you have MelonLoader.
