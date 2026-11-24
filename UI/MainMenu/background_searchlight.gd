extends Polygon2D



func _process(_delta: float) -> void:
	rotation = sin(Time.get_ticks_msec() * 0.001) * 0.2 + 0.4
