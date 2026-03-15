"""
ProjetoPDG (origem em Python) — versão coerente e “limpa”.

Esta versão serve como referência do comportamento pretendido:
- PausDeGiz valida peso (nunca <= 0)
- FabricaDeGiz cria uma lista de paus com peso 1 (total = n)
- Belial calcula n_paus_de_giz e peso_total e permite ajustar peso_total

A migração para C# ficou a meio no projeto principal.
"""

class PausDeGiz:
    def __init__(self, p: int):
        self._peso = 1
        self.peso = p

    @property
    def peso(self) -> int:
        return self._peso

    @peso.setter
    def peso(self, value: int):
        if value <= 0:
            value = 1
        self._peso = value


class FabricaDeGiz:
    def obter_giz(self, total_gramas: int):
        return [PausDeGiz(1) for _ in range(total_gramas)]

    def mudar_peso(self, pau: PausDeGiz, novo_peso: int) -> bool:
        pau.peso = novo_peso
        return pau.peso == novo_peso


class Belial:
    def __init__(self, lista):
        self._a_minha_copia_privada_de_giz = list(lista)

    @property
    def n_paus_de_giz(self) -> int:
        return len(self._a_minha_copia_privada_de_giz)

    @n_paus_de_giz.setter
    def n_paus_de_giz(self, value: int):
        if self.n_paus_de_giz != value:
            self._a_minha_copia_privada_de_giz = FabricaDeGiz().obter_giz(value)

    @property
    def peso_total(self) -> int:
        return sum(pau.peso for pau in self._a_minha_copia_privada_de_giz)

    @peso_total.setter
    def peso_total(self, value: int):
        if self.peso_total != value:
            self._a_minha_copia_privada_de_giz = FabricaDeGiz().obter_giz(value)

    def mudar_peso_total(self, novo_peso: int) -> bool:
        if self.peso_total == novo_peso:
            return True

        peso_acumulado = 0
        nova = []

        for pau in self._a_minha_copia_privada_de_giz:
            if peso_acumulado + pau.peso <= novo_peso:
                nova.append(pau)
                peso_acumulado += pau.peso
            else:
                break

        if peso_acumulado < novo_peso:
            nova.append(PausDeGiz(novo_peso - peso_acumulado))

        self._a_minha_copia_privada_de_giz = nova
        return True

    def peso_medio(self) -> float:
        return self.peso_total / self.n_paus_de_giz


def main():
    fabrica = FabricaDeGiz()

    giz_do_dante = fabrica.obter_giz(10)
    print("Obtive corretamente 10 gramas de giz para o cliente Dante.")

    belial = Belial(giz_do_dante)
    print(f"No Belial estão {belial.n_paus_de_giz} paus de giz, que pesam {belial.peso_total} gramas.")
    print(f"Em média, {belial.peso_medio()} gramas por pau.")

    paudegiz = PausDeGiz(10)
    print(f'O pau tem {paudegiz.peso} gramas.')
    fabrica.mudar_peso(paudegiz, 20)
    print(f'O pau tem {paudegiz.peso} gramas. Devia ter 20, tem?')


if __name__ == "__main__":
    main()
