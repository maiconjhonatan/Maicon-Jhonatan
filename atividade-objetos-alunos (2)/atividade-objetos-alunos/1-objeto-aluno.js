const aluno = {
  nome: "Carlos",
  idade: 17,
  notas: [7, 8, 9],
  estaMatriculado: true
};

console.log(aluno.nome); // Carlos (notação com ponto)
console.log(aluno["idade"]); // 17 (notação com colchetes)
aluno.idade = 18; // altera uma propriedade existente
aluno.cidade = "Recife"; // cria uma nova propriedade
console.log(aluno);

// Pergunta 1
// aluno.notas mostra o array inteiro, [7, 8, 9]. Já o aluno.notas[0] pega só
// o primeiro valor, que é o 7, porque a contagem do array começa no 0.
console.log(aluno.notas);
console.log(aluno.notas[0]);

// Pergunta 2
// O valor novo é 18, porque eu troquei o 17 quando fiz aluno.idade = 18.
console.log(aluno.idade);

// Pergunta 3
// Na prática não muda nada, os dois mostram "Carlos". É só outro jeito de
// escrever. Os colchetes eu preciso usar quando o nome da propriedade está
// guardado numa variável, tipo const campo = "nome"; aluno[campo]. Com o ponto
// isso não dá certo, porque ele ia procurar uma propriedade chamada "campo".
// Também precisa de colchetes quando o nome tem espaço, tipo
// aluno["data de nascimento"].
const campo = "nome";
console.log(aluno[campo]);

// Pergunta 4
// Criei a propriedade curso e mostrei o objeto de novo. Agora aparece também
// a cidade e o curso junto com o resto.
aluno.curso = "Programação";
console.log(aluno);
