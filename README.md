

# Assumptions

You have Dapr installed and some kind of Pubsub

see [Dapr Docs](https://docs.dapr.io/developing-applications/building-blocks/pubsub/howto-publish-subscribe/#step-1-setup-the-pubsub-component).


# Installation

Assume a secret was created
```
$ kubectl create secret generic awxpassword --from-literal=AWX_PASSWORD='SomeAWXPassword'
secret/awxpassword created
```

The username is set in the deployment.yaml.

# Testing

You'll want to use properly formatted JSON messages in Pubsub

e.g.
```json

asdf

```

Otherwise, you can test with a modified version of [the Dapr tutorial React form](https://github.com/dapr/quickstarts/tree/master/tutorials/pub-sub/react-form) as I did.

In my examples, I'm usign a Pubsub topic of "BBB"

# Future plans

The topic of "BBB" is still hardcoded in Program.cs. I want to move that to an env var as well.