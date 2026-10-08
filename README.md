# Kampen om Grønland

Satirisk 2D endless runner lavet i Unity 6 (6000.0.60f1).

Trump kører i en slæde trukket af JD Vance over indlandsisen for at overtage Grønland.
Undgå forhindringerne, saml bitcoins og køb hjælp i Det Ovale Kontor.
Opbakning er dit liv: rammer den 0, bliver du ikke genvalgt.

## Styring

| Tast | Hvad den gør |
|---|---|
| **W / S** | Skift spor |
| **O** | Åbn Det Ovale Kontor. Spillet pauser |
| **Shift** | Brug den opgradering, du har købt i Det Ovale Kontor |
| **Enter / piletaster** | Vælg i menuerne |

## På isen

**Forhindringer**

| | Koster |
|---|---|
| Sæl | 10% opbakning |
| Klimaforsker | 15% opbakning |
| Mette Frederiksen | 30% opbakning. Hun vandrer hurtigt mellem sporene |

**Saml op**

| | Giver |
|---|---|
| Bitcoin | 10 bitcoins |
| Jeff Bezos | 200 bitcoins |
| MAGA-cap | 2% opbakning |
| Pingvin | 5% opbakning |
| Nobels fredspris | 20% opbakning |

Opbakningen kan højst være 100%.

## Det Ovale Kontor

Tryk **O** på isen for at gå ind i Det Ovale Kontor. Spillet står stille, mens du er
derinde, og du fortsætter, hvor du slap, når du går tilbage til isen.

Her kan du købe hjælp for dine bitcoins. Du kan kun have **én mand i lommen ad gangen**,
og du bruger ham med **Shift**, når det passer dig.

| Mand | Pris | Når du trykker Shift |
|---|---|---|
| **Elon Musk** | 300 | Raketmotor: alt kører dobbelt så hurtigt i 5 sekunder, og afstanden tæller dobbelt. Ofte mere skade end gavn |
| **Kim Jong-un** | 400 | Wildcard: 70% chance for +50% opbakning, 30% chance for at miste al opbakning på stedet |

Når du bruger dem, skriver de til dig i en chatboks nederst på skærmen.

## Highscore

Spillet husker den længste afstand, du har kørt. Den vises i hovedmenuen og på
slutskærmen, og slår du rekorden, står der **NY REKORD!**

## Kør projektet

Åbn mappen i Unity Hub med Unity 6000.0.60f1 og start scenen `Assets/Scenes/MainMenu.unity`.

Scenerne er `MainMenu`, `Bane` og `OvaleKontor`. Kontoret lægges oven på `Bane`, når
man trykker O, så det kan ikke startes alene.
