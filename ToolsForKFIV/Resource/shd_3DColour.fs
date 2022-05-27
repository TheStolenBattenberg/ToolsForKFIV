#version 330 core
layout(location = 0) out vec4 cDiffuse;

in vec4 fColour;

void main()
{
    cDiffuse = fColour;
}