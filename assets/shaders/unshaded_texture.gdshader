shader_type spatial;

render_mode unshaded, shadows_disabled;

uniform sampler2D sourceImage : source_color, filter_nearest;
uniform float UVScale;
uniform vec3 albedoGain;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	ALBEDO = texture(sourceImage, UV * UVScale).xyz + albedoGain;
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
