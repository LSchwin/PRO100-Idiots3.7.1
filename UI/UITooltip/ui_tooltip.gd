class_name UITooltip extends Node2D


static func create(text: String, pos: Vector2, _owner: Node) -> UITooltip:
	var tooltip: UITooltip = preload("uid://dkeubmu3ibs5n").instantiate()
	_owner.add_child(tooltip)
	tooltip.setup(text, pos)
	return tooltip


@onready var background: ColorRect = $Background
@onready var label: RichTextLabel = $Label
var tween: Tween

func setup(text: String, pos: Vector2) -> void:
	if tween: tween.kill()
	tween = create_tween()
	tween.tween_property(self, "modulate:a", 1.0, 0.1).from(0.0)
	
	label.text = ""
	label.reset_size()
	label.text = text
	global_position = pos
	await get_tree().process_frame
	background.size = label.size + Vector2(4,4)
	
	var screen_rect := get_viewport_rect()
	var self_rect := Rect2(global_position, label.size)
	if self_rect.end.x > screen_rect.end.x:
		position.x += screen_rect.end.x - self_rect.end.x
	if self_rect.end.y > screen_rect.end.y:
		position.y += screen_rect.end.y - self_rect.end.y

func delete() -> void:
	if tween: tween.kill()
	tween = create_tween()
	tween.tween_property(self, "modulate:a", 0.0, 0.1)
	await tween.finished
	queue_free()
