#!/usr/bin/env python3
"""Generate monster-style enemy sprites for Night Swarm using skia-python."""

from pathlib import Path
import math
import skia

SIZE = 64
OUT_DIR = Path("Assets/Art/Characters/Enemies")


def paint(color, style=skia.Paint.kFill_Style, stroke=1, aa=True):
    p = skia.Paint(AntiAlias=aa, Color=color, Style=style)
    if style == skia.Paint.kStroke_Style:
        p.setStrokeWidth(stroke)
    return p


def rgba(r, g, b, a=255):
    return skia.ColorSetARGB(a, r, g, b)


def save_surface(surface: skia.Surface, name: str):
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    image = surface.makeImageSnapshot()
    data = image.encodeToData()
    (OUT_DIR / name).write_bytes(bytes(data))


def clear(canvas):
    canvas.clear(skia.ColorTRANSPARENT)


def add_shadow(canvas, cx, cy, w, h, alpha=90):
    canvas.drawOval(
        skia.Rect.MakeXYWH(cx - w / 2, cy - h / 2, w, h),
        paint(rgba(5, 4, 12, alpha)),
    )


def draw_teeth(canvas, x, y, count=4, spacing=4, size=4):
    for i in range(count):
        tx = x + i * spacing
        path = skia.Path()
        path.moveTo(tx, y)
        path.lineTo(tx + size / 2, y + size)
        path.lineTo(tx + size, y)
        path.close()
        canvas.drawPath(path, paint(rgba(232, 228, 220, 230)))


def enemy_basic():
    s = skia.Surface(SIZE, SIZE)
    c = s.getCanvas()
    clear(c)

    add_shadow(c, 32, 51, 34, 8)

    # Body
    c.drawRoundRect(skia.Rect.MakeXYWH(14, 12, 36, 38), 16, 16, paint(rgba(67, 26, 84)))
    c.drawRoundRect(skia.Rect.MakeXYWH(18, 16, 28, 28), 12, 12, paint(rgba(108, 44, 130, 220)))

    # Horns
    horn = paint(rgba(43, 18, 58))
    p1 = skia.Path(); p1.moveTo(20, 16); p1.lineTo(16, 6); p1.lineTo(26, 14); p1.close(); c.drawPath(p1, horn)
    p2 = skia.Path(); p2.moveTo(44, 16); p2.lineTo(48, 6); p2.lineTo(38, 14); p2.close(); c.drawPath(p2, horn)

    # Eyes + brows
    c.drawOval(skia.Rect.MakeXYWH(20, 24, 10, 8), paint(rgba(245, 87, 87)))
    c.drawOval(skia.Rect.MakeXYWH(34, 24, 10, 8), paint(rgba(245, 87, 87)))
    c.drawCircle(24, 28, 2, paint(rgba(20, 8, 20)))
    c.drawCircle(38, 28, 2, paint(rgba(20, 8, 20)))
    c.drawLine(19, 22, 30, 22, paint(rgba(30, 10, 40), style=skia.Paint.kStroke_Style, stroke=2))
    c.drawLine(34, 22, 45, 22, paint(rgba(30, 10, 40), style=skia.Paint.kStroke_Style, stroke=2))

    # Mouth and teeth
    c.drawRoundRect(skia.Rect.MakeXYWH(21, 35, 22, 9), 5, 5, paint(rgba(28, 10, 24, 240)))
    draw_teeth(c, 23, 36, count=4, spacing=4, size=4)

    # Claws
    claw = paint(rgba(225, 214, 208, 235))
    for x in (13, 18, 46, 51):
        path = skia.Path()
        path.moveTo(x, 44)
        path.lineTo(x + (2 if x < 32 else -2), 50)
        path.lineTo(x + (4 if x < 32 else -4), 44)
        path.close()
        c.drawPath(path, claw)

    save_surface(s, "enemy_basic.png")


def enemy_shard():
    s = skia.Surface(SIZE, SIZE)
    c = s.getCanvas()
    clear(c)

    add_shadow(c, 32, 53, 30, 6)

    # Crystalline body
    body = skia.Path()
    body.moveTo(32, 8)
    body.lineTo(50, 22)
    body.lineTo(46, 44)
    body.lineTo(32, 56)
    body.lineTo(18, 44)
    body.lineTo(14, 22)
    body.close()
    c.drawPath(body, paint(rgba(26, 102, 128, 235)))

    inner = skia.Path()
    inner.moveTo(32, 14)
    inner.lineTo(44, 24)
    inner.lineTo(40, 40)
    inner.lineTo(32, 48)
    inner.lineTo(24, 40)
    inner.lineTo(20, 24)
    inner.close()
    c.drawPath(inner, paint(rgba(82, 191, 209, 220)))

    # Face fracture + eyes
    crack = paint(rgba(11, 44, 56), style=skia.Paint.kStroke_Style, stroke=2)
    c.drawLine(32, 18, 30, 27, crack)
    c.drawLine(30, 27, 36, 33, crack)
    c.drawLine(36, 33, 31, 43, crack)
    c.drawLine(31, 43, 34, 50, crack)

    c.drawOval(skia.Rect.MakeXYWH(22, 26, 9, 7), paint(rgba(186, 246, 255)))
    c.drawOval(skia.Rect.MakeXYWH(33, 26, 9, 7), paint(rgba(186, 246, 255)))
    c.drawCircle(26, 29, 2, paint(rgba(6, 28, 32)))
    c.drawCircle(37, 29, 2, paint(rgba(6, 28, 32)))

    # Jaw shards
    jaw = paint(rgba(195, 245, 255, 220))
    for i, x in enumerate([24, 28, 32, 36, 40]):
        h = 4 + (i % 2)
        p = skia.Path()
        p.moveTo(x, 40)
        p.lineTo(x + 1.5, 40 + h)
        p.lineTo(x + 3, 40)
        p.close()
        c.drawPath(p, jaw)

    save_surface(s, "enemy_shard.png")


def enemy_warden():
    s = skia.Surface(SIZE, SIZE)
    c = s.getCanvas()
    clear(c)

    add_shadow(c, 32, 53, 38, 7)

    # Heavy armored body
    c.drawRoundRect(skia.Rect.MakeXYWH(10, 12, 44, 40), 10, 10, paint(rgba(42, 50, 66)))
    c.drawRoundRect(skia.Rect.MakeXYWH(14, 16, 36, 32), 8, 8, paint(rgba(68, 79, 100)))

    # Helmet
    c.drawRoundRect(skia.Rect.MakeXYWH(16, 14, 32, 16), 8, 8, paint(rgba(34, 40, 54)))
    c.drawRoundRect(skia.Rect.MakeXYWH(18, 18, 28, 10), 5, 5, paint(rgba(18, 24, 34)))

    # Eyes slit
    c.drawRoundRect(skia.Rect.MakeXYWH(22, 20, 20, 6), 3, 3, paint(rgba(245, 85, 74)))
    c.drawCircle(28, 23, 1.5, paint(rgba(255, 210, 190)))
    c.drawCircle(36, 23, 1.5, paint(rgba(255, 210, 190)))

    # Shoulder spikes
    spike_color = paint(rgba(95, 106, 130))
    for x, direction in [(12, -1), (52, 1)]:
        p = skia.Path()
        p.moveTo(x, 26)
        p.lineTo(x + 8 * direction, 20)
        p.lineTo(x + 3 * direction, 30)
        p.close()
        c.drawPath(p, spike_color)

    # Mouth grille
    c.drawRoundRect(skia.Rect.MakeXYWH(22, 34, 20, 10), 3, 3, paint(rgba(24, 28, 38)))
    grille = paint(rgba(92, 108, 136), style=skia.Paint.kStroke_Style, stroke=1.3)
    for x in [25, 29, 33, 37, 41]:
        c.drawLine(x, 36, x, 42, grille)

    save_surface(s, "enemy_warden.png")


def enemy_echo():
    s = skia.Surface(SIZE, SIZE)
    c = s.getCanvas()
    clear(c)

    add_shadow(c, 32, 54, 28, 6)

    # Ghost layers
    for i, a in enumerate([130, 170, 210]):
        offset = i * 2
        c.drawOval(skia.Rect.MakeXYWH(17 + offset, 12 + offset, 30, 36), paint(rgba(96, 59, 156, a)))

    # Head shape
    c.drawRoundRect(skia.Rect.MakeXYWH(16, 10, 32, 40), 14, 14, paint(rgba(132, 90, 196, 210)))

    # Face
    c.drawOval(skia.Rect.MakeXYWH(22, 22, 8, 10), paint(rgba(230, 237, 255, 240)))
    c.drawOval(skia.Rect.MakeXYWH(34, 22, 8, 10), paint(rgba(230, 237, 255, 240)))
    c.drawCircle(26, 27, 2.2, paint(rgba(39, 18, 62)))
    c.drawCircle(38, 27, 2.2, paint(rgba(39, 18, 62)))

    mouth = skia.Path()
    mouth.moveTo(24, 39)
    mouth.quadTo(32, 46, 40, 39)
    c.drawPath(mouth, paint(rgba(29, 10, 49), style=skia.Paint.kStroke_Style, stroke=2))

    # Floating tendrils
    tendril = paint(rgba(170, 131, 225, 190), style=skia.Paint.kStroke_Style, stroke=3)
    c.drawLine(20, 45, 14, 54, tendril)
    c.drawLine(44, 45, 50, 54, tendril)

    save_surface(s, "enemy_echo.png")


def main():
    enemy_basic()
    enemy_shard()
    enemy_warden()
    enemy_echo()
    print("Generated enemy sprites:")
    for f in ["enemy_basic.png", "enemy_shard.png", "enemy_warden.png", "enemy_echo.png"]:
        print(" -", OUT_DIR / f)


if __name__ == "__main__":
    main()
