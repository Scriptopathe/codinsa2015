#pragma once
#include "Common.h"


class StateAlterationModelView
{

public: 
	// Représente le type de l'altération d'état.
	StateAlterationType Type;
	// Durée de base de l'altération d'état en secondes
	float BaseDuration;
	// Si Type contient Dash : Obtient ou définit une valeur indiquant si le dash permet traverser les
	// murs.
	bool DashGoThroughWall;
	// Si Type contient Dash : type direction du dash.
	DashDirectionType DashDirType;
	// Valeur flat dubuff / debuff (valeur positive : buff, valeur négative : debuff). La nature du
	// buff dépend de Type.
	float FlatValue;
	// Même que FlatValue, mais en pourcentage de dégâts d'attaque actuels de la source.
	float SourcePercentADValue;
	// Même que FlatValue, mais en pourcentage des HP actuels de la source.
	float SourcePercentHPValue;
	// Même que FlatValue, mais en pourcentage des HP max de la source.
	float SourcePercentMaxHPValue;
	// Même que FlatValue mais en pourcentage de l'armure actuelle de la source.
	float SourcePercentArmorValue;
	// Même que FlatValue, mais en pourcentage de l'AP actuelle de l'entité source.
	float SourcePercentAPValue;
	// Même que FlatValue mais en pourcentage de la RM actuelle de l'entité source.
	float SourcePercentRMValue;
	// Même que FlatValue, mais en pourcentage dedégâts d'attaque actuels del'entité de
	// destination.
	float DestPercentADValue;
	// Même que FlatValue, mais en pourcentage des HP actuels de l'entité de destination.
	float DestPercentHPValue;
	// Même que FlatValue, mais en pourcentage des HP max de l'entité de destination.
	float DestPercentMaxHPValue;
	// Même que FlatValue mais en pourcentage de l'armure actuelle de l'entité de destination.
	float DestPercentArmorValue;
	// Même que FlatValue, mais en pourcentage de l'AP actuelle de l'entité de destination.
	float DestPercentAPValue;
	// Même que FlatValue mais en pourcentage de la RM actuelle de l'entité de destination.
	float DestPercentRMValue;
	// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée à une structure.
	float StructureBonus;
	// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée sur un monstre neute.
	float MonsterBonus;
	// Obtient le multiplicateur de dégâts lorsque l'altération est appliquée sur un Virus.
	float VirusBonus;
	void serialize(std::ostream& output);

	static StateAlterationModelView deserialize(std::istream& input);
	StateAlterationModelView();
private: 

};