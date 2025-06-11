# Feedback - Avaliação Geral

## Front End

### Navegação
  * Pontos positivos:
    - Projeto MVC com CRUD funcional para entidades (produtos, categorias).
    - Identity funcional no MVC com login e registro operacionais.

  * Pontos negativos:
    - No MVC, a edição de produto permite alteração do vendedor, o que deveria ser proibido.

### Design
  - Interface funcional e simples, apropriada para uso administrativo.

### Funcionalidade
  * Pontos positivos:
    - CRUD implementado tanto na API quanto no MVC.
    - Identity funcional nas duas camadas.
    - A API cria o vendedor no mesmo momento do cadastro do usuário, com ID compartilhado.
    - A API usa SQLite com migrations automáticas e seed de dados corretamente configurados.

  * Pontos negativos:
    - Na API, os produtos não são vinculados ao usuário logado.
    - No MVC, o produto pode ser atribuído a qualquer vendedor na edição.
    - MVC não possui seed automático de dados.
    - `JwtSettings` está armazenado em uma camada compartilhada ao invés de estar apenas na API, contrariando boas práticas de isolamento de responsabilidade.

## Back End

### Arquitetura
  * Pontos positivos:
    - Separação em três camadas: API, MVC, camada compartilhada (comum).
    - Utilização correta de injeção de dependência e organização modular.

### Funcionalidade
  * Pontos positivos:
    - CRUD e autenticação funcionam corretamente.

  * Pontos negativos:
    - Falta de vínculo entre produto e usuário logado na API e falta de restrição no MVC quanto à propriedade do produto.

### Modelagem
  * Pontos positivos:
    - Entidades simples e bem organizadas.
    - Coerência com o domínio do projeto.

  * Pontos negativos:
    - Nenhum diretamente relacionado à modelagem.

## Projeto

### Organização
  * Pontos positivos:
    - Estrutura modular, com separação entre projetos, documentação presente.

  * Pontos negativos:
    - Versionamento de arquivos de ambiente (.vs, *.suo, caches) que não deveriam estar no repositório.
    - README.md incorreto quanto à descrição do projeto.

### Documentação
  * Pontos positivos:
    - `FEEDBACK.md` presente.

  * Pontos negativos:
    - README.md descreve outro projeto, o que pode causar confusão.

### Instalação
  * Pontos positivos:
    - API executa com SQLite e configurações adequadas.

  * Pontos negativos:
    - Seed não é automatizado no MVC.

---

# 📊 Matriz de Avaliação de Projetos

| **Critério**                   | **Peso** | **Nota** | **Resultado Ponderado**                  |
|-------------------------------|----------|----------|------------------------------------------|
| **Funcionalidade**            | 30%      | 9        | 2,7                                      |
| **Qualidade do Código**       | 20%      | 7        | 1,4                                      |
| **Eficiência e Desempenho**   | 20%      | 9        | 1,8                                      |
| **Inovação e Diferenciais**   | 10%      | 8        | 0,8                                      |
| **Documentação e Organização**| 10%      | 8        | 0,8                                      |
| **Resolução de Feedbacks**    | 10%      | 8        | 0,8                                      |
| **Total**                     | 100%     | -        | **8,3**                                  |

## 🎯 **Nota Final: 8,3 / 10**
