-- Small seed for demonstration. Run these SQL statements against pokemon.db (SQLite).
BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS Pokemon (
  Id INTEGER PRIMARY KEY,
  Name TEXT NOT NULL,
  Generation INTEGER NOT NULL,
  Legendary INTEGER NOT NULL DEFAULT 0
);
CREATE TABLE IF NOT EXISTS PokemonTypes (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  PokemonId INTEGER NOT NULL,
  TypeName TEXT NOT NULL,
  Slot INTEGER NOT NULL,
  FOREIGN KEY(PokemonId) REFERENCES Pokemon(Id) ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS Stats (
  PokemonId INTEGER PRIMARY KEY,
  Hp INTEGER, Attack INTEGER, Defense INTEGER,
  SpecialAttack INTEGER, SpecialDefense INTEGER, Speed INTEGER,
  FOREIGN KEY(PokemonId) REFERENCES Pokemon(Id) ON DELETE CASCADE
);
-- Insert sample Pokémon
INSERT OR IGNORE INTO Pokemon (Id, Name, Generation, Legendary) VALUES (1, 'Bulbasaur', 1, 0);
INSERT OR IGNORE INTO Pokemon (Id, Name, Generation, Legendary) VALUES (4, 'Charmander', 1, 0);
INSERT OR IGNORE INTO Pokemon (Id, Name, Generation, Legendary) VALUES (7, 'Squirtle', 1, 0);
INSERT OR IGNORE INTO Pokemon (Id, Name, Generation, Legendary) VALUES (25, 'Pikachu', 1, 0);
-- Types
INSERT INTO PokemonTypes (PokemonId, TypeName, Slot) VALUES (1, 'Planta', 1);
INSERT INTO PokemonTypes (PokemonId, TypeName, Slot) VALUES (1, 'Veneno', 2);
INSERT INTO PokemonTypes (PokemonId, TypeName, Slot) VALUES (4, 'Fuego', 1);
INSERT INTO PokemonTypes (PokemonId, TypeName, Slot) VALUES (7, 'Agua', 1);
INSERT INTO PokemonTypes (PokemonId, TypeName, Slot) VALUES (25, 'Eléctrico', 1);
-- Stats
INSERT OR IGNORE INTO Stats (PokemonId, Hp, Attack, Defense, SpecialAttack, SpecialDefense, Speed) VALUES (1, 45, 49, 49, 65, 65, 45);
INSERT OR IGNORE INTO Stats (PokemonId, Hp, Attack, Defense, SpecialAttack, SpecialDefense, Speed) VALUES (4, 39, 52, 43, 60, 50, 65);
INSERT OR IGNORE INTO Stats (PokemonId, Hp, Attack, Defense, SpecialAttack, SpecialDefense, Speed) VALUES (7, 44, 48, 65, 50, 64, 43);
INSERT OR IGNORE INTO Stats (PokemonId, Hp, Attack, Defense, SpecialAttack, SpecialDefense, Speed) VALUES (25, 35, 55, 40, 50, 50, 90);
COMMIT;
