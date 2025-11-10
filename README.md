# GeometryTP2

## Scripts
Un seul script EXO1 qui fait tout sur ce tp. Facilite la manipulation du mesh qui n'a pas besoin d'être transmis dans tout les sens.


### Méthodes
meshFromOff: Méthode de l'exercice 1 qui permet d'obtenir un mesh en mettant en entrée un array de ligne extrait d'un .off

getCenter/centerMesh: Méthodes de l'exercice 2, utilisée ensembles, ces méthodes permettent de centrer le mesh évitant que son centre soit placé sous son pied

normalizeMesh:Méthode de l'exercice 3; normalise la mesh, mettant toutes ses valeurs entre 1 et -1.

calculateFaceNormals:Méthode de l'exercice 4, prend le mesh en entrée, récupère tout les triangles et pour chaque triangle calcule la normale et le rentre dans une liste de vecteur. La méthode renvoie cette liste (visible dans l'inspecteur mais pas utilisée pour l'instant)

removeFaces: Méthode de l'exercice 5, prend le mesh et une liste d'indice de face à enlever. La méthode ordonne puis inverse cette liste de face pour éviter les problèmes d'indexation puis refait une liste de triangles sans les index donné qu'elle réapplique au mesh.

ExportToOff/ExportWrite: Deux méthodes d'export pour l'exercice 5. Ces méthodes utilisent respectivement StreamWriter et File.WriteAllText. Elles écrivent les informations sur l'extension du fichier, le nombre de sommet/facette, le nombre d'arête (laissé à zéro donc probablement pas nécessaire). Puis la méthode ajouter tout les sommets et enfin tout les triangles, rendant un document identique à celui utilisé en entrée.
