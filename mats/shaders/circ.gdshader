//	Dial shader v2 by Brian Smith (steampunkdemon.itch.io)
//	MIT Licence

shader_type canvas_item;

// Replace the below references to source_color with hint_color if you are using a version of Godot before 4.0.
//uniform vec4 indictor_color : source_color = vec4(0.0, 0.0, 0.0, 1.0);
//uniform vec4 border_color : source_color = vec4(0.5, 0.5, 0.5, 1.0);
//uniform vec4 centre_color : source_color = vec4(0.0, 0.0, 0.0, 1.0);
//uniform vec4 sector1_color : source_color = vec4(0.0, 1.0, 0.0, 1.0);
//uniform vec4 sector2_color : source_color = vec4(1.0, 1.0, 0.0, 1.0);
//uniform vec4 sector3_color : source_color = vec4(1.0, 0.0, 0.0, 1.0);
//uniform vec4 scale_mark_color : source_color = vec4(0.0, 0.0, 0.0, 1.0);

//uniform bool show_edges = false;
//uniform float indicator_angle : hint_range(0.0, 1.0) = 0.0;
//uniform float indicator_length : hint_range(0.1, 1.0) = 0.65;
//uniform float indicator_width : hint_range(0.001, 0.01) = 0.005;
//uniform float border_width : hint_range(0.0, 0.5) = 0.1;
//uniform float centre_width : hint_range(0.0, 0.5) = 0.1;
uniform float sector : hint_range(0.0, 1.0) = 0.5;
//uniform float sector2 : hint_range(0.0, 1.0) = 0.75;
//uniform float sector3 : hint_range(0.0, 1.0) = 0.905;
//uniform float sector_end : hint_range(0.0, 1.0) = 1.0;
//uniform float scale_marks : hint_range(0.0, 20.0, 1.0) = 16.0;
//uniform float major_scale_marks : hint_range(0.0, 20.0, 1.0) = 4.0;
//uniform float scale_mark_width : hint_range(0.01, 0.1) = 0.02;
//uniform float scale_outer : hint_range(0.0, 1.0) = 0.85;
//uniform float scale_minor_inner : hint_range(0.0, 1.0) = 0.75;
//uniform float scale_major_inner : hint_range(0.0, 1.0) = 0.65;

float greater_than(float x, float y) {
  return max(sign(x - y), 0.0);
}
float _sqrt(vec2 v){
	return sqrt(v.x*v.x+v.y*v.y);
}
void fragment() {
	float ang=atan(UV.y,UV.x)+radians(60);
	vec2 uv = UV*2.0-1.0;
	float a = (atan(uv.y, uv.x) + 3.141592656) / 6.283185312;
	//float l = length(uv);

//	Uncomment the following line if you are applying the shader to a TextureRect and using a version of Godot before 4.0.
//	COLOR = texture(TEXTURE,UV);

//	If you do not want to render all the colored sectors remove the appropriate lines from the following block of code.
	//COLOR.rgb = mix(COLOR.rgb, sector3_color.rgb, sector3_color.a * (greater_than(a, sector3) * greater_than(sector_end, a)));
	//COLOR.rgb = mix(COLOR.rgb, sector2_color.rgb, sector2_color.a * (greater_than(a, sector2) * greater_than(sector3, a)));
	
	vec3 bg_clr=COLOR.rgb-vec3(0.2f);
	COLOR.rgb = mix(COLOR.rgb, bg_clr, COLOR.a * (greater_than(a, sector)));

//	If you do not want to render the scale marks remove the following code block.
//	If you do not want to render the major scale marks remove the second following line.
	//COLOR.rgb = mix(COLOR.rgb, scale_mark_color.rgb, scale_mark_color.a * greater_than(abs(mod(a, 1.0 / scale_marks) * scale_marks * 2.0 - 1.0), 1.0 - (scale_mark_width / 6.283185312 / l) * scale_marks) * (greater_than(scale_outer, l) * greater_than(l, scale_minor_inner)));
	//COLOR.rgb = mix(COLOR.rgb, scale_mark_color.rgb, scale_mark_color.a * greater_than(abs(mod(a, 1.0 / major_scale_marks) * major_scale_marks * 2.0 - 1.0), 1.0 - (scale_mark_width / 6.283185312 / l) * major_scale_marks) * (greater_than(scale_minor_inner, l) * greater_than(l, scale_major_inner)));

	//COLOR.rgb = mix(COLOR.rgb, indictor_color.rgb, indictor_color.a * greater_than(indicator_length, l) * greater_than(indicator_width / l, abs(indicator_angle - (a + 1.0 * greater_than(-0.5, a - indicator_angle) - 1.0 * greater_than(a - indicator_angle, 0.5)))));

//	If you do not want to render the centre remove the following line.
	//COLOR.rgb = mix(COLOR.rgb, centre_color.rgb, centre_color.a * greater_than(centre_width, l));

//	If you do not want to render the border remove the following line.
	//COLOR.rgb = mix(COLOR.rgb, border_color.rgb, border_color.a * greater_than(l, 1.0 - border_width));

//	If you do not want the edges to be transparent remove the following line of code.
//	If you always want the edges to be transparent replace the following line of code with:
//	COLOR.a *= greater_than(1.0, l);
	//COLOR.a *= max(sign(1.0 - l), float(show_edges));
}