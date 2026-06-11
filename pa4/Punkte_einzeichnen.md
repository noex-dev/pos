# Punkte (Marker) auf eine Karte einzeichnen — Spickzettel

Kurzanleitung, wie du Städte/Waldwunder als Punkte auf ein Kartenbild zeichnest.
Kernidee: Eine **Geo-Koordinate (Lat/Lng)** wird in eine **Pixel-Position (x/y)**
auf dem Bild umgerechnet. Das ist reiner Dreisatz, keine komplizierte Mathe.

---

## 1. Die Idee in einem Satz

Das Kartenbild zeigt einen Ausschnitt mit bekannten Grenzen (z. B. Österreich:
oben/unten Latitude, links/rechts Longitude). Für jeden Punkt fragst du:
„Wie weit liegt diese Koordinate zwischen den Rändern?" — derselbe Anteil der
Bildbreite/-höhe ist die Pixel-Position.

```
x_pixel = (lng - lng_links) / (lng_rechts - lng_links) * bildBreite
y_pixel = (lat_oben - lat)   / (lat_oben - lat_unten)   * bildHoehe
```

**Warum ist y umgekehrt?** Pixel zählen von **oben** nach unten (y=0 ist oben),
Latitude aber von **unten** nach oben (höhere Lat = weiter nördlich = weiter oben
im Bild). Deshalb steht beim y die Subtraktion andersrum: `lat_oben - lat`.

---

## 2. XAML — Canvas über das Bild legen

Das `Image` und ein `Canvas` kommen in **dieselbe Grid-Zelle**, sodass das Canvas
exakt über dem Bild liegt. Auf das Canvas zeichnest du dann die Punkte.

```xml
<Grid Grid.Row="1" Grid.Column="1">
    <Image x:Name="MapImage">
        <Image.Source>
            <BitmapImage UriSource="Europe.jpg"/>
        </Image.Source>
    </Image>
    <Canvas x:Name="MapCanvas"/>
</Grid>
```

Wichtig: Das `Canvas` steht **nach** dem `Image`, damit es darüber liegt (spätere
Elemente werden in WPF oben drauf gezeichnet).

---

## 3. Code-Behind — Umrechnung + Zeichnen

Die Geo-Grenzen müssen zum verwendeten **Bild** passen. Die Werte hier sind
Österreich (aus der Waldwunder-Angabe). Für ein anderes Bild → andere Grenzen
aus der Aufgabenangabe einsetzen.

```csharp
// Geo-Grenzen des Kartenbilds (Beispiel: Österreich)
const double LatTop    = 49.063175;  // oben
const double LatBottom = 46.308597;  // unten
const double LngLeft   = 9.362383;   // links
const double LngRight  = 17.231941;  // rechts

private void DrawMarkers(List<Worldcity> cities)
{
    MapCanvas.Children.Clear();   // alte Punkte entfernen

    double w = MapCanvas.ActualWidth;
    double h = MapCanvas.ActualHeight;

    foreach (var city in cities)
    {
        if (city.Lat == null || city.Lng == null) continue;

        // Geo -> Pixel (Dreisatz)
        double x = (city.Lng.Value - LngLeft) / (LngRight - LngLeft) * w;
        double y = (LatTop - city.Lat.Value) / (LatTop - LatBottom) * h;

        var dot = new System.Windows.Shapes.Ellipse
        {
            Width  = 10,
            Height = 10,
            Fill   = System.Windows.Media.Brushes.Red
        };

        // -5 = halbe Breite/Höhe, damit der Punkt MITTIG auf der Koordinate sitzt
        System.Windows.Controls.Canvas.SetLeft(dot, x - 5);
        System.Windows.Controls.Canvas.SetTop(dot, y - 5);

        MapCanvas.Children.Add(dot);
    }
}
```

### Aufruf (am Ende von Button_Click, nach dem Run)

```csharp
program.Run(allCities, result);
DrawMarkers(result);
```

---

## 4. Zeile-für-Zeile-Erklärung

- `MapCanvas.Children.Clear()` — bei jedem Klick die alten Punkte löschen, sonst
  stapeln sich die Marker über mehrere Durchläufe.
- `ActualWidth/ActualHeight` — die **tatsächlich gerenderte** Größe des Canvas in
  Pixeln. Damit rechnest du, nicht mit festen Werten, damit es beim Skalieren passt.
- `city.Lat.Value` — `Lat`/`Lng` sind `double?` (nullable) aus dem Scaffolding.
  `.Value` holt den Wert; der `if (... == null) continue;` überspringt Städte ohne
  Koordinaten.
- `Ellipse` — ein Kreis als Marker. `Width/Height = 10` → 10px Durchmesser.
- `Canvas.SetLeft/SetTop` — so positioniert man ein Element auf einem Canvas
  (absolute Pixel). `x - 5` zentriert den 10px-Kreis genau auf der Koordinate.
- `MapCanvas.Children.Add(dot)` — den Punkt sichtbar machen.

---

## 5. Die zwei häufigsten Fehler

### a) Alle Punkte landen in der Ecke (oben links)
Ursache: `ActualWidth`/`ActualHeight` sind beim ersten Aufruf noch **0**, weil das
Layout noch nicht fertig gerendert ist. Dann wird jedes `x`/`y` zu 0.

**Lösungen:**
- Feste Größe am Canvas setzen (`Width="600" Height="400"`) und damit rechnen, **oder**
- Erst im `Loaded`-Event bzw. nach dem ersten Layout zeichnen, **oder**
- Mit `MapImage.ActualWidth/ActualHeight` rechnen, falls das Bild die Maße vorgibt.

### b) Punkte sitzen falsch / verschoben
Ursache: Die Geo-Grenzen (`LatTop` etc.) passen **nicht zum Bild**. Die Werte oben
sind für ein Österreich-Bild. Für `Europe.jpg` oder ein anderes Bild brauchst du die
Grenzen **dieses** Bildes (stehen in der Aufgabenangabe).

---

## 6. Sauberes Design — wichtig fürs Verständnis

Das Einzeichnen gehört **nicht** in die einzelnen Sprach-Befehle (Expressions).
Trennung:

- **Expressions** entscheiden, *welche* Städte ins `result` kommen (Grammatik-Logik).
- **MainWindow** entscheidet, *wie* `result` dargestellt wird (ListBox + Marker).

Also: alle Marker werden **zentral** im `MainWindow` gezeichnet, indem über die
fertige `result`-Liste iteriert wird — egal ob die Stadt von `LARGEST`, `RANDOM`
oder `SELECT` stammt. Ein Sprachbefehl soll Daten produzieren, nicht zeichnen.

---

## 7. Optional: Marker anklickbar machen (Bonus)

Falls die Aufgabe „Klick auf Marker selektiert den Eintrag in der ListBox" verlangt:

```csharp
dot.Tag = city;                  // Stadt am Punkt merken
dot.MouseLeftButtonDown += (s, e) =>
{
    var c = (Worldcity)((System.Windows.Shapes.Ellipse)s).Tag;
    ErgebnisListBox.SelectedItem = c;   // in der ListBox auswählen
};
```

Über `Tag` hängst du das Datenobjekt an das visuelle Element; im Klick-Handler
holst du es wieder raus und selektierst es in der Liste.
