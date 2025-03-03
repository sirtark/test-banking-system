namespace OriginSolutions.Services
{
    internal sealed class AliasGenService
    {
        readonly string[] words =
        [
            // A (1–36)
            "acabar", "acceso", "acero", "acido", "actor",          // 5
            "actua", "adios", "agudo", "agua", "alamo",              // 10
            "alba", "alce", "aldea", "alma", "alta",                 // 15
            "alza", "amado", "amigo", "amor", "ancho",               // 20
            "andar", "animal", "anual", "apoyo", "apuro",            // 25
            "arbol", "arena", "arma", "arte", "asilo",               // 30
            "asir", "astro", "atomo", "audio", "aureo",              // 35
            "autor",                                               // 36

            // B (37–55)
            "bajo", "bala", "balon", "banco", "banda",               // 5 (37–41)
            "barco", "barra", "bazar", "bebe", "bicho",              // 10 (42–46)
            "bien", "blusa", "bomba", "bravo", "breve",              // 15 (47–51)
            "brisa", "bruto", "burro", "busto",                      // 19 (52–55)

            // C (56–94)
            "cabra", "cacao", "cacto", "calma", "calor",             // 5 (56–60)
            "campo", "canto", "carga", "carro", "carta",             // 10 (61–65)
            "casa", "casco", "cazar", "ceder", "cenar",              // 15 (66–70)
            "censo", "cerdo", "cerca", "cielo", "cifra",             // 20 (71–75)
            "cinco", "circo", "citar", "clave", "clima",             // 25 (76–80)
            "cobra", "coche", "cola", "color", "combo",              // 30 (81–85)
            "coral", "corto", "creer", "credo", "criar",             // 35 (86–90)
            "crudo", "cubo", "curar", "curva",                       // 39 (91–94)

            // D (95–112)
            "dado", "dar", "dardo", "deber", "dejar",                // 5 (95–99)
            "delta", "denso", "dieta", "dicha", "dicho",             // 10 (100–104)
            "diente", "digno", "dinero", "diosa", "doble",           // 15 (105–109)
            "donar", "dos", "drama",                                // 18 (110–112)

            // E (113–127)
            "eco", "edad", "ejido", "eje", "el",                      // 5 (113–117)
            "ella", "ello", "empleo", "enano", "enigma",             // 10 (118–122)
            "indice", "inmune", "instar", "ir", "isla",              // 15 (123–127)

            // F (128–150)
            "fama", "fango", "farol", "fatal", "favor",              // 5 (128–132)
            "feo", "feliz", "feroz", "fervor", "fiar",               // 10 (133–137)
            "ficha", "fideo", "fijar", "firma", "flaco",             // 15 (138–142)
            "fluir", "fondo", "forma", "frase", "frio",              // 20 (143–147)
            "fruta", "fuego", "fulgor",                             // 23 (148–150)

            // G (151–173)
            "gallo", "gama", "ganar", "ganso", "garza",              // 5 (151–155)
            "gastar", "gato", "gemir", "genio", "gente",             // 10 (156–160)
            "girar", "globo", "golfo", "golpe", "grano",             // 15 (161–165)
            "grato", "grave", "grito", "grupo", "guapo",             // 20 (166–170)
            "guion", "gusto", "gozar",                              // 23 (171–173)

            // H (174–190)
            "haba", "hacer", "hacha", "hada", "halo",                // 5 (174–178)
            "harpa", "harto", "haz", "hielo", "hierro",              // 10 (179–183)
            "hiena", "hijo", "hombre", "honor", "huella",            // 15 (184–188)
            "hueso", "hurto",                                       // 17 (189–190)

            // I (191–202)
            "idea", "ideal", "igual", "imagen", "impar",             // 5 (191–195)
            "indice", "inmune", "instar", "ir", "isla",              // 10 (196–200)
            "icono", "islar",                                       // 12 (201–202)

            // J (203–208)
            "jabon", "jarra", "jefe", "jugar", "junto", "justo",      // 6 (203–208)

            // K (209–210)
            "kilo", "karma",                                      // 2 (209–210)

            // L (211–236)
            "labio", "lacio", "lago", "laja", "lama",                // 5 (211–215)
            "lana", "lata", "lado", "largo", "larva",                // 10 (216–220)
            "leal", "leche", "leo", "lento", "letra",                // 15 (221–225)
            "libre", "lirio", "llama", "llano", "llave",             // 20 (226–230)
            "lleva", "lima", "lince", "liso", "lista",               // 25 (231–235)
            "lloro",                                              // 26 (236)

            // M (237–268)
            "macho", "madre", "mafia", "magma", "mal",               // 5 (237–241)
            "malo", "mano", "marca", "marco", "marea",               // 10 (242–246)
            "mar", "matar", "media", "medio", "mejor",               // 15 (247–251)
            "mesa", "metro", "mixto", "miedo", "milla",              // 20 (252–256)
            "mina", "mismo", "molar", "molde", "monto",              // 25 (257–261)
            "moral", "morir", "mosca", "mover", "mucha",             // 30 (262–266)
            "mundo", "mural",                                     // 32 (267–268)

            // N (269–278)
            "nada", "nacer", "nacio", "nadar", "nueve",              // 5 (269–273)
            "nuevo", "nube", "nulo", "nuez", "nino",                 // 10 (274–278)

            // O (279–291)
            "obra", "obrar", "obvio", "oceano", "ocaso",             // 5 (279–283)
            "octavo", "odio", "ojo", "olmo", "onda",                 // 10 (284–288)
            "opaco", "opina", "optar",                              // 13 (289–291)

            // P (292–335)
            "pacto", "pagar", "paila", "palo", "pan",                // 5 (292–296)
            "panda", "papel", "parar", "pardo", "parir",             // 10 (297–301)
            "parte", "pasar", "paseo", "pasta", "pauta",             // 15 (302–306)
            "pecar", "peces", "pedir", "pegar", "peine",             // 20 (307–311)
            "pelea", "pelar", "pelo", "peor", "perla",               // 25 (312–316)
            "perro", "pesa", "pesar", "pez", "pieza",                // 30 (317–321)
            "picar", "pico", "pilar", "pinta", "pique",              // 35 (322–326)
            "plato", "playa", "pleno", "pluma", "pobre",             // 40 (327–331)
            "poder", "podar", "polvo", "poner",                     // 44 (332–335)

            // Q (336–342)
            "queda", "queja", "quema", "quedo", "quien",            // 5 (336–340)
            "quito", "queso",                                       // 7 (341–342)

            // R (343–369)
            "racha", "radio", "rama", "rango", "rapto",              // 5 (343–347)
            "rasgo", "rastro", "razon", "reina", "reino",            // 10 (348–352)
            "remar", "renta", "rezar", "rico", "rinde",              // 15 (353–357)
            "ritmo", "robar", "roble", "rodeo", "rogar",             // 20 (358–362)
            "rollo", "roto", "rubor", "rueda", "ruido",              // 25 (363–367)
            "rural",                                              // 26 (368–369)

            // S (370–401)
            "sable", "sacar", "sacro", "salir", "salmo",             // 5 (370–374)
            "salon", "salsa", "santo", "savia", "secar",             // 10 (375–379)
            "segar", "seis", "sello", "senal", "serio",              // 15 (380–384)
            "sesgo", "siete", "silla", "silva", "simio",             // 20 (385–389)
            "sismo", "sobre", "sol", "soler", "solos",               // 25 (390–394)
            "sombra", "sonar", "suave", "sucio", "suma",             // 30 (395–399)
            "super", "sur",                                       // 32 (400–401)

            // T (402–427)
            "tabla", "tacto", "talon", "tango", "tapar",             // 5 (402–406)
            "tarde", "techo", "temor", "tenor", "tensa",             // 10 (407–411)
            "tener", "termo", "teson", "tigre", "tocar",             // 15 (412–416)
            "tomar", "topar", "torno", "torpe", "toser",             // 20 (417–421)
            "total", "trigo", "trio", "trono", "trapio", "tonto",    // 26 (422–427)

            // U (428–434)
            "una", "uno", "urdir", "urgir", "usado",                 // 5 (428–432)
            "usar", "usura", "utero",                               // 8 (433–434)

            // V (435–461)
            "vaca", "vagar", "vaina", "vale", "valor",               // 5 (435–439)
            "vapor", "varon", "vasto", "veces", "verde",             // 10 (440–444)
            "verbo", "verso", "vestir", "vetar", "vicar",             // 15 (445–449)
            "vicio", "vida", "video", "vigor", "vimos",              // 20 (450–454)
            "vinil", "vino", "viril", "visar", "viste",              // 25 (455–459)
            "vital", "vivir",                                       // 27 (460–461)

            // W (462–463)
            "wafle", "web",                                        // 2 (462–463)

            // X (464)
            "xenon",                                              // 1 (464)

            // Y (465–468)
            "yate", "yerno", "yugos", "yunque",                     // 4 (465–468)

            // Z (469–500)
            // Grupo "za" (12)
            "zafar", "zafio", "zafiro", "zaino", "zambo", 
            "zanco", "zanja", "zanjar", "zampa", "zapata", 
            "zarpa", "zarza",                                      // 12 (469–480)
            // Grupo "ze" (4)
            "zebra", "zelar", "zenit", "zeta",                      // 4 (481–484)
            // Grupo "zi" (1)
            "zigzag",                                             // 1 (485)
            // Grupo "zo" (6)
            "zoco", "zocalo", "zombi", "zona", "zonzo", "zorro",    // 6 (486–491)
            // Grupo "zu" (8)
            "zuavo", "zueco", "zulo", "zurcir", "zurdo", "zumba", "zumo", "zurrar"  // 8 (492–499)
            // Total Z: 12+4+1+6+8 = 31; acumulado hasta 499 (0-based index 499 = elemento 500)
        ];
        public string GenerateAlias()
            => string.Join('.', Random.Shared.GetItems(words, 3)).ToUpper();
    }
}
