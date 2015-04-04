#pragma once
#include "Common.h"
#include "StateAlterationModelView.h"
#include "StateAlterationParametersView.h"


class StateAlterationView
{

public: 
	// Id de la source de l'altération d'état.
	int Source;
	// Représente le type de source de l'altération d'état.
	StateAlterationSource SourceType;
	// Représente le modèle d'altération d'état appliquée sur une entité.
	StateAlterationModelView Model;
	// Représente les paramètres de l'altération d'état.
	StateAlterationParametersView Parameters;
	// Temps restant en secondes pour l'altération d'état.
	float RemainingTime;
	void serialize(std::ostream& output);

	static StateAlterationView deserialize(std::istream& input);
	StateAlterationView();
private: 

};