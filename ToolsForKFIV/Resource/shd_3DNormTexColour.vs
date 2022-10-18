#version 330 core
layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec3 vNormal;
layout (location = 2) in vec2 vTexcoord;
layout (location = 3) in vec4 vColour;

out vec3 fNormal;
out vec2 fTexcoord;
out vec4 fColour;

uniform mat4 worldMatrix;
uniform mat4 cameraMatrix;

void main()
{
    gl_Position = (cameraMatrix * worldMatrix) * vec4(vPosition, 1.0f);

	fNormal = normalize(vNormal);
	fTexcoord = vTexcoord;
    fColour = vColour;
}