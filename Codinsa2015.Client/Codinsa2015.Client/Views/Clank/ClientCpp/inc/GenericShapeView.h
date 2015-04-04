#pragma once
#include "Common.h"
#include "Vector2.h"


class GenericShapeView
{

public: 
	// Position de la forme : cercle => centre ; rectangle => coin supérieur gauche
	Vector2 Position;
	// Si cercle : rayon du cercle.
	float Radius;
	// Si rectangle : taille du rectangle.
	Vector2 Size;
	// Représente le type de la forme.
	GenericShapeType ShapeType;
	void serialize(std::ostream& output);

	static GenericShapeView deserialize(std::istream& input);
	GenericShapeView();
private: 

};