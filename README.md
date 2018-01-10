Ingenia activity for UiPath
================

This code enables a user to use Ingenia within the UiPath platform. Ingenia automatically categorises content based on the categories, or 'tags', of its user.

For more information about Ingenia: www.ingenia-api.com
API documentation: www.ingenia-api.com/documentation

### SET UP
To begin with, a user will need to have an Ingenia account, which is characterised by:
* an API key
* a unique bundle_id: this is the identifier for the 'bundle' where the user's 'items' will be saved, similar to a folder. A user can have several bundles.

### INPUT
On an ongoing basis, the user can currently pass the following input to Ingenia:
* text (required): a block of text
* tags (optional): a string with the tags that the user wants to associate with the text

Note: Ingenia can also extract text from URLs or files (MS Office, PDF, txt, etc.). The activity could be modified to support these use cases if needed.

Text and tags will be saved on Ingenia's database, which enables several other use cases, as well as the continuous improvement in the accuracy of the categorisation.

### DATA PROCESSING AND OUTPUT
The goal of this activity is for Ingenia to return the most relevant categories for the text, if any, as selected from the pool of categories of this user.

If the user has no categories, Ingenia will return an empty array of tags.

In this context, the user can create categories by sending an item of text with some tags: what's called a 'training item'. 
By doing this, the user will 'train' Ingenia:
* per each tag in the 'tags' string, Ingenia will do a 'find or create', thus creating tags for this user.
* Ingenia will learn that the user wants this text associated with these tags

As soon as at least 4 training items have been sent, Ingenia will automatically start processing the content, aiming to understand what kind of text should be associated with which of the available tags.

From this point onwards, Ingenia is 'ready to categorise': 
* if the user sends a training item, Ingenia will use the new information to re-process the content, thus improving the accuracy of the categorisation over time.
* if the user sends an items without tags, Ingenia will assume it simply needs to be categorised

Either way, Ingenia will send back a response in real-time containing what it considers as the most relevant tags at that point in time, for that text.
* If the item was a training item, this will include at least the 'user assigned' tags, and possibly also some other, 'machine only assigned' tags, that the user hasn't assigned, but that Ingenia considers relevant.
* If the item was a normal item, all the tags are 'machine only assigned' by definition.




