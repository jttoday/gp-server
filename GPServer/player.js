
function __addCard(card){
    this.cards.push(card);
}

function Player(clientId, modelId, position, status){
    this.clientId = clientId;
    this.modelId = modelId;
    this.pos = position;
    this.status = status;
    this.speed = speed;
    this.might = might;
    this.sanity = sanity;
    this.knowledge = knowledge;
    this.cards = [];
    this.addCard = __addCard;
}

module.exports = Player;
