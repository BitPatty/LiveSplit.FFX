/*
	Static offsets:
		Tidus: 		D3205C
		Yuna: 		D320F0
		Auron: 		D32184
		Kimahri: 	D32218
		Wakka: 		D322AC
		Lulu: 		D32340
		Rikku: 		D323D4
		Seymour: 	D32468
		
	Party: D307E8 
*/

#include <stdint.h>

struct Character {
	uint8_t c1;
	uint8_t c2;
	uint8_t c3;
	uint8_t c4;
	const uint32_t base_hp;
	const uint32_t base_mp;
	const uint8_t base_str;
	const uint8_t base_agil;
	const uint8_t base_def;
	const uint8_t base_mag;
	const uint8_t base_mdef;
	const uint8_t base_luck;
	const uint8_t base_eva;
	const uint8_t base_acc;
	uint8_t c5;
	uint8_t c6;
	uint8_t c7;
	uint8_t c8;
	uint8_t c9;
	uint8_t c10;
	uint8_t c11;
	uint8_t c12;
	uint32_t current_hp; 					//outside battle
	uint32_t current_mp; 					//outside battle 
	uint32_t max_hp; 						//outside battle 
	uint32_t max_mp; 						//outside battle
	uint8_t c13;
	uint8_t current_weapon;
	uint8_t current_armor;
	uint8_t strength;
	uint8_t defense;
	uint8_t magic;
	uint8_t magic_defense;
	uint8_t agility;
	uint8_t luck;
	uint8_t evasion
	uint8_t accuracy;
	uint8_t c14;
	uint8_t current_od_mode;
	uint8_t c15;
	uint8_t c16;
	uint8_t sphere_levels_available;
	uint8_t sphere_levels_spent;
	uint8_t c17;
	std::vector<bool> abilities(12*8);		//1 bit per ability 
	uint8_t c17; 							//00
	uint8_t c18; 							//00
	uint8_t c19; 							//00
	uint8_t c20; 							//00
	uint8_t c21; 							//00
	uint8_t c22; 							//00
	uint8_t c23; 							//00
	uint8_t c24; 							//00
	uint8_t c25; 							//00
	uint8_t c26; 							//00
	uint32_t enemies_killed;				//Used for Tonberry
	uint8_t c27; 							//00 
	uint8_t c28; 							//00
	uint8_t c29; 							//00
	uint8_t c30; 							//00
	uint8_t c31; 							//00
	uint8_t c32; 							//00
	uint8_t c33; 							//00
	uint8_t c34; 							//00
	uint16_t od_counter_warrior;			//Countdowns for OD modes 
	uint16_t od_counter_comrade;
	uint16_t od_counter_stoic;
	uint16_t od_counter_healer;
	uint16_t od_counter_tactician;
	uint16_t od_counter_sufferer;
	uint16_t od_counter_sufferer;
	uint16_t od_counter_dancer;
	uint16_t od_counter_avenger;
	uint16_t od_counter_slayer;
	uint16_t od_counter_hero;
	uint16_t od_counter_rook;
	uint16_t od_counter_victor;
	uint16_t od_counter_coward;
	uint16_t od_counter_ally;
	uint16_t od_counter_victim;
	uint16_t od_counter_daredevil;
	uint16_t od_counter_loner;
	uint8_t c35; 							//FF
	uint8_t c36; 							//FF
	uint8_t c37; 							//FF
	uint8_t c38; 							//FF
	uint8_t c39; 							//FF
	uint8_t c40; 							//FF
	uint8_t c41; 							//04, Seymour FF 
	uint8_t c42; 							//00, Seymour FF 
	uint8_t c43; 							//00, Seymour 01 
	uint8_t c44; 							//00
	uint8_t c45; 							//00
	uint8_t c46; 							//00
	uint8_t c47; 							//00
	uint8_t c48; 							//00
	uint8_t c49; 							//00
} Character;