
function __addCard(card){
    this.cards.push(card);
}

function Player(id, modelId, speed, might, sanity, knowledge){
    this.id = id;
    this.modelId = modelId;
    this.speed = speed;
    this.might = might;
    this.sanity = sanity;
    this.knowledge = knowledge;
    this.cards = [];
    this.addCard = __addCard;
}

module.exports = Player;
