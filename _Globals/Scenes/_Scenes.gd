extends CanvasLayer


# Base Godot functions
func _ready() -> void:
	fade_out()


# Loading scenes
@onready var scenes := fetch_scenes()
const SCENES_PATH := "res://_Globals/Scenes/List/"
func fetch_scenes() -> Dictionary[String, PackedScene]:
	var dict: Dictionary[String, PackedScene] = {}
	for file in ResourceLoader.list_directory(SCENES_PATH):
		var resource := load(SCENES_PATH + file)
		if resource is not SceneResource: continue
		dict.set(resource.scene_name, resource.scene_packed)
	return dict


# Swapping
func swap(scene: String, _fade_in := 0.25, _fade_out := 0.25) -> void:
	if not scenes.has(scene):
		push_error(scene + " does not exist in our SCENES dictionary!")
		return
	
	Selector.disable()
	await fade_in(_fade_in)
	get_tree().change_scene_to_packed(scenes.get(scene))
	await fade_out(_fade_out)


# Fading
@onready var fader: ColorRect = $Fader
var tween: Tween

func fade_in(time := 0.25) -> Signal:
	return _fade(1.0, time)

func fade_out(time := 0.25) -> Signal:
	return _fade(0.0, time)

func _fade(target_a: float, time := 0.25) -> Signal:
	fader.show()
	if tween: tween.kill()
	tween = create_tween()
	
	tween.tween_property(fader, "color:a", target_a, time)
	tween.tween_callback(fader.hide)
	return tween.finished
