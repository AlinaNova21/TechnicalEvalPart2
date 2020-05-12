<template>
  <div class="home">
    <v-text-field label="search" v-model="search" />
    <v-alert color="info" dark v-if="!contacts.length">No contacts in address book</v-alert>
    <v-card v-for="contact in contacts" :key="contact.contactId" class="d-inline-block ma-2">
      <v-card-title class="subheading">{{ contact.fullName }}</v-card-title>
      <v-divider />
      <v-list dense>
        <v-list-item v-if="contact.street">
          <v-list-item-content>Address:</v-list-item-content>
          <v-list-item-content class="align-end">
            {{ contact.street }}<br />
            {{ contact.city }}, {{ contact.state }} {{ contact.zip }}
          </v-list-item-content>
        </v-list-item>
        <v-list-item v-if="contact.homePhone">
          <v-list-item-content>Home Phone:</v-list-item-content>
          <v-list-item-content class="text-align-right align-end">{{ formatPhone(contact.homePhone) }}</v-list-item-content>
        </v-list-item>
        <v-list-item v-if="contact.mobilePhone">
          <v-list-item-content>Mobile Phone:</v-list-item-content>
          <v-list-item-content class="text-align-right align-end">{{ formatPhone(contact.mobilePhone) }}</v-list-item-content>
        </v-list-item>
        <v-list-item v-if="contact.workPhone">
          <v-list-item-content>Work Phone:</v-list-item-content>
          <v-list-item-content class="text-align-right align-end">{{ formatPhone(contact.workPhone) }}</v-list-item-content>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-btn text @click="editContact(contact.contactId)">Edit</v-btn>
        <v-btn text color="red" @click="deleteContact(contact.contactId)">Delete</v-btn>
      </v-card-actions>
    </v-card>
    <v-dialog v-model="dialog">
      <template #activator="{ on }">
        <v-btn color="primary" fixed dark fab bottom right v-on="on">
          <v-icon>mdi-plus</v-icon>
        </v-btn>
      </template>
      <EditContact v-if="dialog" @saved="saved()" :id="editId" />
    </v-dialog>
  </div>
</template>

<script>
// @ is an alias to /src
import { computed, onMounted, ref, watch } from '@vue/composition-api'
import EditContact from '@/components/EditContact.vue'
import { wrapStore } from '@/lib/utils'
import debounce from 'lodash/debounce'

export default {
  name: 'home',
  components: {
    EditContact
  },
  setup (props, context) {
    const { $store } = context.root
    const contacts = computed(() => $store.state.addressbook.contacts)
    const Contact = wrapStore($store, 'addressbook', 'contactId')
    const refresh = () => Contact.list()
    const deleteContact = async (id) => {
      await Contact.remove(id)
      await refresh()
    }
    const formatPhone = p => p.replace(/(\d{3})(\d{3})(\d{4})/, '($1) $2-$3')
    onMounted(refresh)
    const dialog = ref(false)
    const editId = ref(0)
    watch(() => dialog.value, v => {
      if (!v) editId.value = 0
    })
    const editContact = id => {
      editId.value = id
      dialog.value = true
    } 
    const saved = () => {
      refresh()
      dialog.value = false
    }
    const search = ref('')
    watch(() => search.value, debounce(searchString => Contact.list({ searchString }), 500))
    return { contacts, dialog, deleteContact, editId, editContact, formatPhone, refresh, saved, search }
  }
}
</script>
