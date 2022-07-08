# TransactionApp
####  DISCUSSION: In the case of the third amount (50030), did the debit amount correspond to the expected amount? 

* If Yes, How? - Explain
* If No, Why? - Is there anything that can be done to remedy the situation.

####  Answer
I am assuming the Expected Amount also means the Transfer amount. I believe that the debit amount corresponds to the <br/>
expected amount because using the configuration data, the charge for amount greater than 50000 attracts a charge of 50. So 50030<br/>
will yield a transfer amount of 49980. I am of the opinion that if my algorithm is right, the output will be <b>*right*</b> too since the <br/>
charge is read from the configuration file.
