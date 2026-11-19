extends CanvasLayer


const SCENES := { # PRO-TIP: Press RIGHT-CLICK on a .tscn file to get a scene's UID!
	"Main Menu":          preload("uid://cjjanmmuxihyw"),
	"Testing Menu":       preload("uid://bs7my58g5w75l"),
	"Test Level":         preload("uid://lr6u3ffle3we"),
	"UI Testing":         preload("uid://bs0jti4w6bys"),
	"Movement Test Room": preload("uid://bb8h1jcbv1yw1"),
	"Base Level":         preload("uid://cuxghuk5ns5q2"),
	#"": preload(""),
	#"": preload(""),
	#"": preload(""),
	#"": preload(""),
	#"": preload(""),
}


func _ready() -> void:
	fade_out()


func swap(scene: String, _fade_in := 0.25, _fade_out := 0.25) -> void:
	Selector.disable()
	await fade_in(_fade_in)
	get_tree().change_scene_to_packed(SCENES.get(scene))
	await fade_out(_fade_out)


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
