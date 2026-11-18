const aluno = {
  nome: "Carlos",
  idade: 17,
  notas: [7, 8, 9],
  calcularMedia: function () {
    let soma = 0;
    for (let nota of this.notas) {
      soma += nota;
    }
    return soma / this.notas.length;
  },
  apresentar: function () {
    return "Meu nome é " + this.nome + " e tenho " + this.idade + " anos.";
  },
  adicionarNota: function (nota) {
    this.notas.push(nota);
  }
};

console.log(aluno.calcularMedia()); // 8

// Pergunta 5
// Porque "notas" sozinho é uma variável que não existe dentro da função, então
// dá erro. O this.notas pega o array notas do próprio objeto que chamou o
// método, que no caso é o aluno.

// Pergunta 6
// Usei this.nome e this.idade para montar a frase. Tive que colocar idade: 17
// no objeto, porque nesse exemplo ele não tinha idade.
console.log(aluno.apresentar()); // Meu nome é Carlos e tenho 17 anos.

// Pergunta 7
// O push coloca a nota nova no final do array. Depois de adicionar o 10, as
// notas ficam [7, 8, 9, 10] e a média sobe para 8.5.
aluno.adicionarNota(10);
console.log(aluno.calcularMedia()); // 8.5
